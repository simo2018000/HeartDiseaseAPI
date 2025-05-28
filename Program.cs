// In Program.cs
using HeartDiseaseAPI.Mapping;
using HeartDiseaseAPI.Models; // For Patient, MongoDbSettings, ModelSettings
using HeartDiseaseAPI.Services;
using Microsoft.Extensions.Options; // For IOptions

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDbSettings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Configure ModelSettings
builder.Services.Configure<ModelSettings>(
    builder.Configuration.GetSection("ModelSettings")); // Added for ONNX model path

// Register services
builder.Services.AddSingleton<MyMongoService>();
builder.Services.AddSingleton<PatientServices>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddSingleton<PredictionService>(); // Ensure this is the correct, consolidated service

// Add controllers
builder.Services.AddControllers(); // Required for controllers to work

// Add Swagger/OpenAPI (Optional but recommended)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware for enabling request buffering (was already here)
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});
// The second Use block for EnableBuffering was removed as it's redundant.

app.UseHttpsRedirection(); // Added for HTTPS

// app.UseAuthorization(); // Add this if you implement authentication/authorization

app.MapControllers(); // This maps attribute-routed controllers (like PatientController)

app.MapGet("/", () => "Heart Disease API is running!");



app.Run();