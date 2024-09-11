using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.MenuItems.Dtos;
using TasteTrailOwnerExperience.Core.MenuItems.Services;
using TasteTrailOwnerExperience.Core.Users.Dtos;

namespace TasteTrailOwnerExperience.Api.MenuItems.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> CreateAsync([FromForm] MenuItemCreateDto menuItem)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var menuItemId = await _menuItemService.CreateMenuItemAsync(menuItem, userInfo);

            // Setting default image
            await _menuItemService.SetMenuItemImageAsync(menuItemId, userInfo, null);

            return Ok(menuItemId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> SetImageAsync(int menuItemId, IFormFile image)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(image);

            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _menuItemService.SetMenuItemImageAsync(menuItemId, userInfo, image);

            return Ok(imageUrl);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> DeleteByIdAsync(int menuItemId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var menuItem = await _menuItemService.GetMenuItemByIdAsync(menuItemId);

            if (menuItem is null)
                return NotFound(menuItemId);

            await _menuItemService.DeleteMenuItemImageAsync(menuItem.Id, userInfo);
            var deletedId = await _menuItemService.DeleteMenuItemByIdAsync(menuItemId, userInfo);

            return Ok(deletedId);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> DeleteImageAsync(int menuItemId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _menuItemService.DeleteMenuItemImageAsync(menuItemId, userInfo);

            return Ok(imageUrl);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> UpdateAsync([FromForm] MenuItemUpdateDto menuItem)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var menuItemId = await _menuItemService.PutMenuItemAsync(menuItem, userInfo!);

            if (menuItemId is null)
                return NotFound(menuItemId);

            return Ok(menuItemId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
