using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PatientServices>();

var app = builder.Build();

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
    var result = service.Update(id, updated);
    return result ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/patients/{id}", (int id, PatientServices service) =>
{
    var result = service.Delete(id);
    return result ? Results.NoContent() : Results.NotFound();
});

app.Run();
