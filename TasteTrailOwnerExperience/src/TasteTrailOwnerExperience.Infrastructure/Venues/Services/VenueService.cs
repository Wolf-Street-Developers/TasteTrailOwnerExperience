using Microsoft.AspNetCore.Http;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailOwnerExperience.Core.Common.Exceptions;
using TasteTrailOwnerExperience.Core.Menus.Services;
using TasteTrailOwnerExperience.Core.Users.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Models;
using TasteTrailOwnerExperience.Core.Venues.Repositories;
using TasteTrailOwnerExperience.Core.Venues.Services;
using TasteTrailOwnerExperience.Infrastructure.Venues.Managers;

namespace TasteTrailOwnerExperience.Infrastructure.Venues.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;

    private readonly IMenuService _menuService;

    private readonly VenueImageManager _venueImageManager;

    public VenueService(IVenueRepository venueRepository,  VenueImageManager venueImageManager, IMenuService menuService)
    {
        _venueRepository = venueRepository;
        _venueImageManager = venueImageManager;
        _menuService = menuService;
    }

    public async Task<Venue?> GetVenueByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var venue = await _venueRepository.GetByIdAsync(id);

        return venue;
    }

    public async Task<int> CreateVenueAsync(VenueCreateDto venue, UserInfoDto userInfo)
    {
        var newVenue = new Venue() {
            Name = venue.Name,
            Address = venue.Address,
            Longtitude = venue.Longtitude,
            Latitude = venue.Latitude,
            Description = venue.Description,
            Email = venue.Email,
            ContactNumber = venue.ContactNumber,
            AveragePrice = venue.AveragePrice,
            UserId = userInfo.Id,
            CreationDate = DateTime.UtcNow,
        };

        var venueId = await _venueRepository.CreateAsync(newVenue);

        return venueId;
    }

    public async Task<int?> DeleteVenueByIdAsync(int id, UserInfoDto userInfo)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var venue = await _venueRepository.GetAsNoTrackingAsync(id);

        if (venue is null)
            return null;

        var isAdmin = userInfo.Role == UserRoles.Admin.ToString();

        if (!isAdmin && venue.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        // Deleting all nested
        await _menuService.DeleteMenuImagesByVenueIdAsync(venue.Id, userInfo);
        var venueId = await _venueRepository.DeleteByIdAsync(id);

        return venueId;
    }

    public async Task<int?> PutVenueAsync(VenueUpdateDto venue, UserInfoDto userInfo)
    {
        var venueToUpdate = await _venueRepository.GetAsNoTrackingAsync(venue.Id);

        if (venueToUpdate is null)
            return null;

        var isAdmin = userInfo.Role == UserRoles.Admin.ToString();

        if (!isAdmin && venueToUpdate.UserId != userInfo.Id)
            throw new ForbiddenAccessException();

        var updatedVenue = new Venue() {
            Id = venue.Id,
            Name = venue.Name,
            Address = venue.Address,
            Longtitude = venue.Longtitude,
            Latitude = venue.Latitude,
            Description = venue.Description,
            Email = venue.Email,
            ContactNumber = venue.ContactNumber,
            AveragePrice = venue.AveragePrice,
            UserId = userInfo.Id,
            CreationDate = DateTime.UtcNow,
        };

        var venueId = await _venueRepository.PutAsync(updatedVenue);

        return venueId;
    }

    public async Task<string?> SetVenueImageAsync(int venueId, UserInfoDto userInfo, IFormFile? image) 
    {
        var venue = await _venueRepository.GetByIdAsync(venueId);

        if (venue is null)
            return null;

        if (venue.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _venueImageManager.SetImageAsync(venue.Id, image);

        return imageUrl;
    }

    public async Task<string?> DeleteVenueImageAsync(int venueId, UserInfoDto userInfo) 
    {
        var venue = await _venueRepository.GetByIdAsync(venueId);

        if (venue is null)
            return null;

        if (venue.UserId != userInfo.Id)
            throw new ForbiddenAccessException();
        
        var imageUrl = await _venueImageManager.DeleteImageAsync(venue.Id);

        return imageUrl;
    }
}
