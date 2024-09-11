using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TasteTrailExperience.Infrastructure.Common.Options;

namespace TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;


public static class InitAuthMethod
{
    public static void InitAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtLifeTimeInMinutes = Environment.GetEnvironmentVariable("JWT_LIFE_TIME_IN_MINUTES");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");


        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtLifeTimeInMinutes)
        || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience)) {
            throw new ArgumentException("Cannot find JWT configurations in environment variables.");
        }

        configuration["Jwt:Key"] = jwtKey;
        configuration["Jwt:LifeTimeInMinutes"] = jwtLifeTimeInMinutes;
        configuration["Jwt:Issuer"] = jwtIssuer;
        configuration["Jwt:Audience"] = jwtAudience;

        var jwtSection = configuration.GetSection("Jwt");
        serviceCollection.Configure<JwtOptions>(jwtSection);
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = jwtSection.Get<JwtOptions>() ?? throw new Exception("cannot find Jwt Section");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtOptions!.KeyInBytes)
                };
            });

        serviceCollection.AddAuthorization();
    }
}
