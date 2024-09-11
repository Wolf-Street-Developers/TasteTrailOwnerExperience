using Microsoft.AspNetCore.Http;
using TasteTrailOwnerExperience.Core.MenuItems.Dtos;
using TasteTrailOwnerExperience.Core.MenuItems.Models;
using TasteTrailOwnerExperience.Core.Users.Dtos;

namespace TasteTrailOwnerExperience.Core.MenuItems.Services;

public interface IMenuItemService
{
    Task<MenuItem?> GetMenuItemByIdAsync(int id);

    Task<int> CreateMenuItemAsync(MenuItemCreateDto menuItem, UserInfoDto userInfo);

    Task<int?> DeleteMenuItemByIdAsync(int id, UserInfoDto userInfo);
    
    Task<int?> PutMenuItemAsync(MenuItemUpdateDto menuItem, UserInfoDto userInfo);

    Task<string?> SetMenuItemImageAsync(int menuItemId, UserInfoDto userInfo, IFormFile? image);

    Task<string?> DeleteMenuItemImageAsync(int menuItemId, UserInfoDto userInfo);

    Task<int?> DeleteMenuItemImagesByMenuIdAsync(int menuId, UserInfoDto userInfo);
}
