using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

var builder = WebApplication.CreateBuilder(args);

var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

if (!string.IsNullOrEmpty(postgresConnectionString)) {
    System.Console.WriteLine(postgresConnectionString);
    builder.Configuration["ConnectionStrings:DefaultConnection"] = postgresConnectionString;
}


// Настраиваем DbContext с использованием строки подключения из конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OwnerExperienceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();