// In Program.cs
using HeartDiseaseAPI.Mapping;
using HeartDiseaseAPI.Models; // For Patient, MongoDbSettings
using HeartDiseaseAPI.Services;
using Microsoft.Extensions.Options; // For IOptions

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDbSettings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register services
builder.Services.AddSingleton<MyMongoService>(); 
builder.Services.AddSingleton<PatientServices>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile)); 
builder.Services.AddSingleton<PredictionService>();


var app = builder.Build();

// Middleware for enabling request buffering (was already here)
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});
// This second Use block for EnableBuffering seems redundant if the one above exists. You likely only need it once.
// app.Use(async (ctx, next) =>
// {
// ctx.Request.EnableBuffering();
// await next();
// });

app.MapGet("/", () => "Heart Disease API is running!");

// Adjust patient endpoints for async and string ID
app.MapGet("/patients", async (PatientServices service) => // Add async
{
    return Results.Ok(await service.GetAllAsync()); // Call GetAllAsync
});

app.MapGet("/patients/{id}", async (string id, PatientServices service) => // id is now string, add async
{
    var patient = await service.GetByIdAsync(id); // Call GetByIdAsync
    return patient is not null ? Results.Ok(patient) : Results.NotFound();
});

app.MapPost("/patients", async (Patient patient, PatientServices service) => // add async
{
    // Assuming this endpoint is for creating a patient directly without DTOs/hashing
    // (as per original Program.cs structure).
    // If patient.Id is null, MongoDB will generate it.
    await service.AddPatientAsync(patient); // Call new AddPatientAsync
    return Results.Created($"/patients/{patient.Id}", patient); // patient.Id is now a string
});

app.MapPut("/patients/{id}", async (string id, Patient updated, PatientServices service) => // id is now string, add async
{
    // The original check "if (id != updated.ID)" might be problematic if updated.ID is an int
    // and id is a string. Ensure IDs are compared correctly (both as strings).
    // If updated.Id is null after deserialization from request body, you can assign it:
    if (updated.Id == null) updated.Id = id;
    else if (id != updated.Id) // Compare string IDs
    {
        return Results.BadRequest("ID in the URL does not match ID in the body.");
    }

    var result = await service.UpdateAsync(id, updated); // Call UpdateAsync
    return result ? Results.NoContent() : Results.NotFound();
});


app.MapDelete("/patients/{id}", async (string id, PatientServices service) => // id is now string, add async
{
    var result = await service.DeleteAsync(id); // Call DeleteAsync
    return result ? Results.NoContent() : Results.NotFound();
});

app.Run();