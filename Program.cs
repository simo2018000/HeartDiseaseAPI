// In Program.cs
using HeartDiseaseAPI.Mapping;
using HeartDiseaseAPI.Models; // For Patient, MongoDbSettings, ModelSettings
using HeartDiseaseAPI.Services;
using Microsoft.Extensions.Options; // For IOptions
using Microsoft.AspNetCore.StaticFiles; // Add this using directive for static files

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

// Add CORS policy setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOrigin", // You can name your policy
        builder => builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "https://localhost:7225") // <--- IMPORTANT: Replace with your React app's URL
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials()); // If you use cookies or credentials
});

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

// Middleware for enabling request buffering
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

// Use CORS policy - This must be called before UseHttpsRedirection and UseAuthorization
app.UseCors("AllowFrontendOrigin");

// --- Static file serving middleware (must be before MapControllers) ---
app.UseDefaultFiles(); // Serves index.html for the root path
app.UseStaticFiles();  // Serves other static files (JS, CSS, images, etc.)

app.UseHttpsRedirection(); // Added for HTTPS
// app.UseAuthorization(); // If enabled

app.MapControllers(); // Maps your API endpoints (e.g., /api/patient)

// This line will only be hit if UseDefaultFiles/UseStaticFiles/MapControllers don't find a match
app.MapGet("/", () => "Heart Disease API is running!");

// --- ADD THIS LINE FOR SPA CLIENT-SIDE ROUTING ---
// For any unhandled routes, fallback to index.html (allowing React Router to take over)
app.MapFallbackToFile("index.html");

app.Run();