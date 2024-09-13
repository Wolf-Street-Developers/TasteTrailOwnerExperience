using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.Menus.Dtos;
using TasteTrailOwnerExperience.Core.Menus.Services;
using TasteTrailOwnerExperience.Core.Users.Dtos;

namespace TasteTrailOwnerExperience.Api.Menus.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }


    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> CreateAsync([FromForm] MenuCreateDto menu)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };
            
            var menuId = await _menuService.CreateMenuAsync(menu, userInfo);

            return Ok(menuId);
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
    public async Task<IActionResult> SetImageAsync(int menuId, IFormFile image)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(image);

            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _menuService.SetMenuImageAsync(menuId, userInfo, image);

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
    public async Task<IActionResult> DeleteByIdAsync(int menuId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var menu = await _menuService.GetMenuByIdAsync(menuId);

            if (menu is null)
                return NotFound(menuId);

            
            await _menuService.DeleteMenuImageAsync(menu.Id, userInfo);
            var deletedId = await _menuService.DeleteMenuByIdAsync(menuId, userInfo);

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
    public async Task<IActionResult> DeleteImageAsync(int menuId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _menuService.DeleteMenuImageAsync(menuId, userInfo);

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
    public async Task<IActionResult> UpdateAsync([FromForm] MenuUpdateDto menu)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var menuId = await _menuService.PutMenuAsync(menu, userInfo);

            if (menuId is null)
                return NotFound(menuId);

            return Ok(menuId);
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
