using System.Runtime.InteropServices;
using dotenv.net;

namespace TasteTrailOwnerExperience.Api.Common.Utilities;

public static class SetupEnvironmentVariables
{
    public static void SetupEnvironmentVariablesMethod(IConfiguration configuration, bool IsDevelopment)
    {
        var options = new DotEnvOptions(envFilePaths: ["../../../.env"]);
        DotEnv.Load(options);

        string? postgresConnectionString;

        // Setup Db Connection String
        if (IsDevelopment) {
            var postgresHost = "localhost";
            var postgresPort = "4500";
            var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var postgresDb = Environment.GetEnvironmentVariable("POSTGRES_DB");

            postgresConnectionString = $"Host={postgresHost};Port={postgresPort};Username={postgresUser};Password={postgresPassword};Database={postgresDb};Pooling=true;";
        }
        else
            postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

        System.Console.WriteLine(postgresConnectionString);

        if (!string.IsNullOrEmpty(postgresConnectionString)) {
            configuration["ConnectionStrings:PostgresConnection"] = postgresConnectionString;
        }

        // Setup Azure Blob Storage Connection String
        string? blobStorageConnectionString;

        if (IsDevelopment) {
            var azureBlobDefaultProtocol = Environment.GetEnvironmentVariable("AZURE_BLOB_DEFAULT_PROTOCOL");
            var azureBlobAccountName = Environment.GetEnvironmentVariable("AZURE_BLOB_ACCOUNT_NAME");
            var azureBlobAccountKey = Environment.GetEnvironmentVariable("AZURE_BLOB_ACCOUNT_KEY");
            var azureBlobEndpointSuffix = Environment.GetEnvironmentVariable("AZURE_BLOB_ENDPOINT_SUFFIX");

            blobStorageConnectionString = $"DefaultEndpointsProtocol={azureBlobDefaultProtocol};AccountName={azureBlobAccountName};AccountKey={azureBlobAccountKey};EndpointSuffix={azureBlobEndpointSuffix}";
        }
        else
            blobStorageConnectionString = Environment.GetEnvironmentVariable("BLOB_STORAGE_CONNECTION_STRING");


        if (!string.IsNullOrEmpty(blobStorageConnectionString)) {
            configuration["ConnectionStrings:AzureBlobStorage"] = blobStorageConnectionString;
        }

        // Setup JWT
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
    }
}
