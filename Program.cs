using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
