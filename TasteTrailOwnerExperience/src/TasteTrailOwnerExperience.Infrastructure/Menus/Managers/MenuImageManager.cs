using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using TasteTrailData.Infrastructure.Blob.Managers;
using TasteTrailOwnerExperience.Core.Menus.Repositories;

public class MenuImageManager : BaseBlobImageManager<int>
{
    private readonly IMenuRepository _menuRepository;

    private readonly string _defaultImageUrl;

    public MenuImageManager(BlobServiceClient blobServiceClient, IMenuRepository menuRepository)
        : base(blobServiceClient, "menu-images")
    {
        _menuRepository = menuRepository;
        _defaultImageUrl = GetDefaultImageUrl();
    }

    public async override Task<string> SetImageAsync(int menuId, IFormFile? image)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId) ?? throw new ArgumentException($"Menu with ID {menuId} not found.");

        if (image == null || image.Length == 0) 
        {
            if (menu.ImageUrlPath is null)
            {
                menu.ImageUrlPath = _defaultImageUrl;
                await _menuRepository.PutAsync(menu);

                return _defaultImageUrl;
            }

            return menu.ImageUrlPath;
        }

        // Create blob container if it doesn't exist
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = $"{menu.Id}{Path.GetExtension(image.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
        }

        var imageUrl = blobClient.Uri.ToString();

        menu.ImageUrlPath = imageUrl;
        await _menuRepository.PutAsync(menu);

        return menu.ImageUrlPath;
    }

    public async override Task<string> DeleteImageAsync(int menuId)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId) ?? throw new ArgumentException($"Menu with ID {menuId} not found.");

        // If image isn't already default
        if (!string.IsNullOrEmpty(menu.ImageUrlPath) && !menu.ImageUrlPath.Equals(_defaultImageUrl, StringComparison.OrdinalIgnoreCase))
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobUri = new Uri(menu.ImageUrlPath).AbsolutePath.TrimStart('/');
            var blobName = Path.GetFileName(blobUri);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }


        menu.ImageUrlPath = _defaultImageUrl;
        await _menuRepository.PutAsync(menu);

        return menu.ImageUrlPath;
    }
}
