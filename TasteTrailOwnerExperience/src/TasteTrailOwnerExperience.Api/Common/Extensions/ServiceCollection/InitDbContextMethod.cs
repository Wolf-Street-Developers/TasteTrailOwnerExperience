using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

namespace TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;

public static class InitDbContextMethod
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

        if (!string.IsNullOrEmpty(postgresConnectionString)) {
            configuration["ConnectionStrings:PostgresConnection"] = postgresConnectionString;
        }

        var connectionString = configuration.GetConnectionString("PostgresConnection");
        serviceCollection.AddDbContext<OwnerExperienceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
