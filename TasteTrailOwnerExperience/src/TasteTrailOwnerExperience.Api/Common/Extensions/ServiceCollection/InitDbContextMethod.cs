using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

namespace TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;

public static class InitDbContextMethod
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection");
        serviceCollection.AddDbContext<OwnerExperienceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
