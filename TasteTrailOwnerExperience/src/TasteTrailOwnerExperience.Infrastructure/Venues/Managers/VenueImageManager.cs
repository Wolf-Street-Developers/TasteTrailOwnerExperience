using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using TasteTrailData.Infrastructure.Blob.Managers;
using TasteTrailOwnerExperience.Core.Venues.Repositories;

namespace TasteTrailOwnerExperience.Infrastructure.Venues.Managers;

public class VenueImageManager : BaseBlobImageManager<int>
{
    private readonly IVenueRepository _venueRepository;

    private readonly string _defaultImageUrl;


    public VenueImageManager(IVenueRepository venueRepository, BlobServiceClient blobServiceClient) 
        : base(blobServiceClient, "venue-images")
    {
        _venueRepository = venueRepository;
        _defaultImageUrl = GetDefaultImageUrl();
    }

    public async override Task<string> SetImageAsync(int venueId, IFormFile? image)
    {
        var venue = await _venueRepository.GetByIdAsync(venueId) ?? throw new ArgumentException($"Venue with ID {venueId} not found.");

        if (image == null || image.Length == 0) 
        {
            if (venue.LogoUrlPath is null)
            {
                venue.LogoUrlPath = _defaultImageUrl;
                await _venueRepository.PutAsync(venue);

                return _defaultImageUrl;
            }

            return venue.LogoUrlPath;
        }

        // Create blob container if it doesn't exist
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = $"{venue.Id}{Path.GetExtension(image.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
        }

        var imageUrl = blobClient.Uri.ToString();

        venue.LogoUrlPath = imageUrl;
        await _venueRepository.PutAsync(venue);

        return imageUrl;
    }

    public override async Task<string> DeleteImageAsync(int venueId)
    {
        var venue = await _venueRepository.GetByIdAsync(venueId) ?? throw new ArgumentException($"Venue with ID {venueId} not found.");

        // If image isn't already default
        if (!string.IsNullOrEmpty(venue.LogoUrlPath) && !venue.LogoUrlPath.Equals(_defaultImageUrl, StringComparison.OrdinalIgnoreCase))
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobUri = new Uri(venue.LogoUrlPath).AbsolutePath.TrimStart('/');
            var blobName = Path.GetFileName(blobUri);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        venue.LogoUrlPath = _defaultImageUrl;
        await _venueRepository.PutAsync(venue);

        return venue.LogoUrlPath;
    }
}
