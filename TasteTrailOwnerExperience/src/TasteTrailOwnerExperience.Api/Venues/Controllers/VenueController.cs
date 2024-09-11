using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.Users.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Services;

namespace TasteTrailOwnerExperience.Api.Venues.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class VenueController : Controller
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRoles.Admin)},{nameof(UserRoles.Owner)}")]
    public async Task<IActionResult> CreateAsync([FromForm] VenueCreateDto venue)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };
            
            var venueId = await _venueService.CreateVenueAsync(venue, userInfo);

            // Setting default image
            await _venueService.SetVenueImageAsync(venueId, userInfo, null);

            return Ok(venueId);
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
    public async Task<IActionResult> SetImageAsync(int venueId, IFormFile image)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(image);

            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _venueService.SetVenueImageAsync(venueId, userInfo, image);

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
    public async Task<IActionResult> DeleteByIdAsync(int venueId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var venue = await _venueService.GetVenueByIdAsync(venueId);

            if (venue is null)
                return NotFound(venueId);

            await _venueService.DeleteVenueImageAsync(venue.Id, userInfo);
            var deletedId = await _venueService.DeleteVenueByIdAsync(venue.Id, userInfo);

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
    public async Task<IActionResult> DeleteImageAsync(int venueId)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var imageUrl = await _venueService.DeleteVenueImageAsync(venueId, userInfo);

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
    public async Task<IActionResult> UpdateAsync([FromForm] VenueUpdateDto venue)
    {
        try
        {
            var userInfo =  new UserInfoDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
            };

            var venueId = await _venueService.PutVenueAsync(venue, userInfo);

            if (venueId is null)
                return NotFound(venueId);

            return Ok(venueId);
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
