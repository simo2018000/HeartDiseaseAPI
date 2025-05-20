using HeartDiseaseAPI.Mapping;
using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PatientServices>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();

});
app.Use(async (ctx, next) =>
{
    ctx.Request.EnableBuffering();
    await next();
});

app.MapGet("/", () => "Heart Disease API is running!");

app.MapGet("/patients", (PatientServices service) =>
{
    return Results.Ok(service.GetAll());
});

app.MapGet("/patients/{id}", (int id, PatientServices service) =>
{
    var patient = service.GetById(id);
    return patient is not null ? Results.Ok(patient) : Results.NotFound();
});

app.MapPost("/patients", (Patient patient, PatientServices service) =>
{
    var created = service.Add(patient);
    return Results.Created($"/patients/{created.ID}", created);
});

app.MapPut("/patients/{id}", (int id, Patient updated, PatientServices service) =>
{
    if (id != updated.ID)
    {
        return Results.BadRequest("ID in the URL does not match ID in the body.");
    }

    var patient = service.GetById(id);
    if (patient is null)
    {
        return Results.NotFound();
    }

    var result = service.Update(id, updated);
    return result ? Results.NoContent() : Results.NotFound();
});


app.MapDelete("/patients/{id}", (int id, PatientServices service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();

});
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var client = new HttpClient(handler);

app.Run();
