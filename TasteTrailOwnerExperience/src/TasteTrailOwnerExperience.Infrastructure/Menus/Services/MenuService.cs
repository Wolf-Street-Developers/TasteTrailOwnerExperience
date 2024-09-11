using Microsoft.AspNetCore.Http;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.MenuItems.Services;
using TasteTrailOwnerExperience.Core.Menus.Dtos;
using TasteTrailOwnerExperience.Core.Menus.Models;
using TasteTrailOwnerExperience.Core.Menus.Repositories;
using TasteTrailOwnerExperience.Core.Menus.Services;
using TasteTrailOwnerExperience.Core.Users.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Repositories;

namespace TasteTrailOwnerExperience.Infrastructure.Menus.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;

    private readonly IVenueRepository _venueRepository;

    private readonly IMenuItemService _menuItemService;

    private readonly MenuImageManager _menuImageManager;

    public MenuService(IMenuRepository menuRepository, IVenueRepository venueRepository, MenuImageManager menuImageManager, IMenuItemService menuItemService)
    {
        _menuRepository = menuRepository;
        _venueRepository = venueRepository;
        _menuImageManager = menuImageManager;
        _menuItemService = menuItemService;
    }

    public async Task<Menu?> GetMenuByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menu = await _menuRepository.GetByIdAsync(id);

        return menu;
    }

    public async Task<int> CreateMenuAsync(MenuCreateDto menu, UserInfoDto userInfo)
    {
        var venue = await _venueRepository.GetByIdAsync(menu.VenueId) ?? 
            throw new ArgumentException($"Venue by ID: {menu.VenueId} not found.");

        if (venue.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        var newMenu = new Menu() {
            Name = menu.Name,
            Description = menu.Description,
            VenueId = venue.Id,
            UserId = userInfo.Id
        };

        var menuId = await _menuRepository.CreateAsync(newMenu);

        return menuId;
    }

    public async Task<int?> DeleteMenuByIdAsync(int id, UserInfoDto userInfo)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menu = await _menuRepository.GetAsNoTrackingAsync(id);

        if (menu is null)
            return null;

        var isAdmin = userInfo.Role == UserRoles.Admin.ToString();

        if (!isAdmin && menu.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        await _menuItemService.DeleteMenuItemImagesByMenuIdAsync(menu.Id, userInfo);
        var menuId = await _menuRepository.DeleteByIdAsync(id);

        return menuId;
    }

    public async Task<int?> PutMenuAsync(MenuUpdateDto menu, UserInfoDto userInfo)
    {
        var menuToUpdate = await _menuRepository.GetAsNoTrackingAsync(menu.Id);

        if (menuToUpdate is null)
            return null;

        if (menuToUpdate.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        var updatedMenu = new Menu() 
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
            UserId = userInfo.Id
        };

        var menuId = await _menuRepository.PutAsync(updatedMenu);

        return menuId;
    }

    public async Task<string?> SetMenuImageAsync(int menuId, UserInfoDto userInfo, IFormFile? image)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);

        if (menu is null)
            return null;

        if (menu.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _menuImageManager.SetImageAsync(menu.Id, image);

        return imageUrl;
    }

    public async Task<string?> DeleteMenuImageAsync(int menuId, UserInfoDto userInfo)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);

        if (menu is null)
            return null;

        if (menu.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _menuImageManager.DeleteImageAsync(menu.Id);

        return imageUrl;
    }

    public async Task<int?> DeleteMenuImagesByVenueIdAsync(int venueId, UserInfoDto userInfo)
    {
        var venue = await _venueRepository.GetByIdAsync(venueId);

        if (venue is null)
            return null;

        if (venue.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var menus = await _menuRepository.GetAllByVenueIdAsync(venue.Id);

        foreach (var menu in menus) {
            await _menuImageManager.DeleteImageAsync(menu.Id);
            await _menuItemService.DeleteMenuItemImagesByMenuIdAsync(menu.Id, userInfo);
        }

        return venueId;
    }
}
