using System.Runtime.InteropServices;
using Azure.Storage.Blobs;

namespace TasteTrailOwnerExperience.Api.Common.Extensions.ServiceCollection;

public static class RegisterBlobStorageMethod
{
    public static void RegisterBlobStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton(sp =>
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            return new BlobServiceClient(connectionString);
        });
    }
}
