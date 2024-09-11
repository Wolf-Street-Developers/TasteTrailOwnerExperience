using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using TasteTrailData.Infrastructure.Blob.Managers;
using TasteTrailOwnerExperience.Core.MenuItems.Repositories;

namespace TasteTrailOwnerExperience.Infrastructure.MenuItems.Managers;

public class MenuItemImageManager : BaseBlobImageManager<int>
{
    private readonly IMenuItemRepository _menuItemRepository;

    private readonly string _defaultImageUrl;


    public MenuItemImageManager(BlobServiceClient blobServiceClient, IMenuItemRepository menuItemRepository)
        : base(blobServiceClient, "menuitem-images")
    {
        _menuItemRepository = menuItemRepository;
        _defaultImageUrl = GetDefaultImageUrl();
    }

    public async override Task<string> SetImageAsync(int menuItemId, IFormFile? image)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId) ?? throw new ArgumentException($"Menu with ID {menuItemId} not found.");

        if (image == null || image.Length == 0) 
        {
            if (menuItem.ImageUrlPath is null)
            {
                menuItem.ImageUrlPath = _defaultImageUrl;
                await _menuItemRepository.PutAsync(menuItem);

                return _defaultImageUrl;
            }

            return menuItem.ImageUrlPath;
        }

        // Create blob container if it doesn't exist
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = $"{menuItem.Id}{Path.GetExtension(image.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
        }

        var imageUrl = blobClient.Uri.ToString();

        menuItem.ImageUrlPath = imageUrl;
        await _menuItemRepository.PutAsync(menuItem);

        return menuItem.ImageUrlPath;
    }

    public async override Task<string> DeleteImageAsync(int menuItemId)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId) ?? throw new ArgumentException($"Menu with ID {menuItemId} not found.");

        // If image isn't already default
        if (!string.IsNullOrEmpty(menuItem.ImageUrlPath) && !menuItem.ImageUrlPath.Equals(_defaultImageUrl, StringComparison.OrdinalIgnoreCase))
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobUri = new Uri(menuItem.ImageUrlPath).AbsolutePath.TrimStart('/');
            var blobName = Path.GetFileName(blobUri);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        menuItem.ImageUrlPath = _defaultImageUrl;
        await _menuItemRepository.PutAsync(menuItem);

        return menuItem.ImageUrlPath;
    }
}
