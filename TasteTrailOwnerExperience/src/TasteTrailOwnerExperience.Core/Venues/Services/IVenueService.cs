using Microsoft.AspNetCore.Http;
using TasteTrailOwnerExperience.Core.Users.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Dtos;
using TasteTrailOwnerExperience.Core.Venues.Models;

namespace TasteTrailOwnerExperience.Core.Venues.Services;

public interface IVenueService
{
    Task<Venue?> GetVenueByIdAsync(int id);

    Task<int> CreateVenueAsync(VenueCreateDto venue, UserInfoDto userInfo);

    Task<int?> DeleteVenueByIdAsync(int id, UserInfoDto userInfo);
    
    Task<int?> PutVenueAsync(VenueUpdateDto venue, UserInfoDto userInfo);

    Task<string?> SetVenueImageAsync(int venueId, UserInfoDto userInfo, IFormFile? image);

    Task<string?> DeleteVenueImageAsync(int venueId, UserInfoDto userInfo);
}
