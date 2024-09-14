using Microsoft.AspNetCore.Http;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.Common.MessageBroker;
using TasteTrailOwnerExperience.Core.MenuItems.Dtos;
using TasteTrailOwnerExperience.Core.MenuItems.Models;
using TasteTrailOwnerExperience.Core.MenuItems.Repositories;
using TasteTrailOwnerExperience.Core.MenuItems.Services;
using TasteTrailOwnerExperience.Core.Menus.Repositories;
using TasteTrailOwnerExperience.Core.Users.Dtos;
using TasteTrailOwnerExperience.Infrastructure.Common.RabbitMq.Dtos;
using TasteTrailOwnerExperience.Infrastructure.MenuItems.Managers;

namespace TasteTrailOwnerExperience.Infrastructure.MenuItems.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;

    private readonly IMenuRepository _menuRepository;

    private readonly MenuItemImageManager _menuItemImageManager;

    private readonly IMessageBrokerService _messageBroker;

    public MenuItemService(IMenuItemRepository menuItemRepository, IMenuRepository menuRepository, MenuItemImageManager  menuItemImageManager, IMessageBrokerService messageBroker)
    {
        _menuItemRepository = menuItemRepository;
        _menuRepository = menuRepository;
        _menuItemImageManager = menuItemImageManager;
        _messageBroker = messageBroker;
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menuItem = await _menuItemRepository.GetByIdAsync(id);

        return menuItem;
    }

    public async Task<int> CreateMenuItemAsync(MenuItemCreateDto menuItem, UserInfoDto userInfo)
    {
        var menu = await _menuRepository.GetByIdAsync(menuItem.MenuId) ?? 
            throw new ArgumentException($"Menu by ID: {menuItem.MenuId} not found.");

        if (menu.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        var newMenuItem = new MenuItem() {
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            MenuId = menu.Id,
            UserId = userInfo.Id
        };

        var menuItemId = await _menuItemRepository.CreateAsync(newMenuItem);

        // Setting default image
        await _menuItemImageManager.SetImageAsync(menuItemId, null);

        await _messageBroker.PushAsync("menuitem_create", newMenuItem);

        return menuItemId;
    }

    public async Task<int?> DeleteMenuItemByIdAsync(int id, UserInfoDto userInfo)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menuItem = await _menuItemRepository.GetAsNoTrackingAsync(id);

        if (menuItem is null)
            return null;

        var isAdmin = userInfo.Role == UserRoles.Admin.ToString();

        if (!isAdmin && menuItem.UserId != userInfo.Id)
            throw new ForbiddenAccessException();


        var menuItemId = await _menuItemRepository.DeleteByIdAsync(id);

        await _messageBroker.PushAsync("menuitem_delete", menuItemId);

        return menuItemId;
    }

    public async Task<int?> PutMenuItemAsync(MenuItemUpdateDto menuItem, UserInfoDto userInfo)
    {
        var menuItemToUpdate = await _menuItemRepository.GetAsNoTrackingAsync(menuItem.Id);

        if (menuItemToUpdate is null)
            return null;

        if (menuItemToUpdate.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        var updatedMenuItem = new MenuItem() {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            UserId = userInfo.Id,
            MenuId = menuItemToUpdate.MenuId
        };

        var menuItemId = await _menuItemRepository.PutAsync(updatedMenuItem);

        await _messageBroker.PushAsync("menuitem_put", updatedMenuItem);

        return menuItemId;
    }

    public async Task<string?> SetMenuItemImageAsync(int menuItemId, UserInfoDto userInfo, IFormFile? image) 
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);

        if (menuItem is null)
            return null;

        if (menuItem.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _menuItemImageManager.SetImageAsync(menuItem.Id, image);
        await _messageBroker.PushAsync("menuitem_set_image", new ImageMessageDto() { ImageUrl = imageUrl, EntityId = menuItemId});

        return imageUrl;
    }

    public async Task<string?> DeleteMenuItemImageAsync(int menuItemId, UserInfoDto userInfo) 
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);

        if (menuItem is null)
            return null;

        if (menuItem.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _menuItemImageManager.DeleteImageAsync(menuItem.Id);
        await _messageBroker.PushAsync("menuitem_delete_image", new ImageMessageDto() { ImageUrl = imageUrl, EntityId = menuItemId});

        return imageUrl;
    }

    public async Task<int?> DeleteMenuItemImagesByMenuIdAsync(int menuId, UserInfoDto userInfo)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);

        if (menu is null)
            return null;

        if (menu.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var menuItems = await _menuItemRepository.GetAllByMenuIdAsync(menu.Id);

        foreach (var menuItem in menuItems)
            await _menuItemImageManager.DeleteImageAsync(menuItem.Id);

        return menuId;
    }
}
