using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Api.Common.Assembly;
using TasteTrailData.Api.Common.Extensions.ServiceCollection;
using TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;
using TasteTrailOwnerExperience.Api.Common.Utilities;
using TasteTrailOwnerExperience.Core.Common.Options;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

var builder = WebApplication.CreateBuilder(args);

SetupEnvironmentVariables.SetupEnvironmentVariablesMethod(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.InitDbContext(builder.Configuration);
builder.Services.InitAuth(builder.Configuration);
builder.Services.InitSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.RegisterBlobStorage(builder.Configuration);
builder.Services.RegisterDependencyInjection();

var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqOptions>(rabbitMqSection);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var assembly = Assembly.GetAssembly(typeof(ApiAssemblyMarker)) ?? throw new InvalidOperationException("Unable to load the assembly containing ApiAssemblyMarker.");

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<OwnerExperienceDbContext>();

    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();