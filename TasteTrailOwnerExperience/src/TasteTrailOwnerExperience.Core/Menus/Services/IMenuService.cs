using Microsoft.AspNetCore.Http;
using TasteTrailOwnerExperience.Core.Menus.Dtos;
using TasteTrailOwnerExperience.Core.Menus.Models;
using TasteTrailOwnerExperience.Core.Users.Dtos;

namespace TasteTrailOwnerExperience.Core.Menus.Services;

public interface IMenuService
{
    Task<Menu?> GetMenuByIdAsync(int id);

    Task<int> CreateMenuAsync(MenuCreateDto menu, UserInfoDto userInfo);

    Task<int?> DeleteMenuByIdAsync(int menuId, UserInfoDto userInfo);
    
    Task<int?> PutMenuAsync(MenuUpdateDto menu, UserInfoDto userInfo);

    Task<string?> SetMenuImageAsync(int menuId, UserInfoDto userInfo, IFormFile? image);

    Task<string?> DeleteMenuImageAsync(int menuId, UserInfoDto userInfo);

    Task<int?> DeleteMenuImagesByVenueIdAsync(int venueId, UserInfoDto userInfo);
}
