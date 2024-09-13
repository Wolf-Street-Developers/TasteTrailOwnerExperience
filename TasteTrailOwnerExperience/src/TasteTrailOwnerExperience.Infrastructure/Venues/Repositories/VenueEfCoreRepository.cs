using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Core.Venues.Models;
using TasteTrailOwnerExperience.Core.Venues.Repositories;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

namespace TasteTrailOwnerExperience.Infrastructure.Venues.Repositories;

public class VenueEfCoreRepository : IVenueRepository
{
    private readonly OwnerExperienceDbContext _dbContext;

    public VenueEfCoreRepository(OwnerExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Venue?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.Venues
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    
    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await _dbContext.Venues
            .FirstOrDefaultAsync(v => v.Id == id);
    }
    
    public async Task<int> CreateAsync(Venue venue)
    {
        ArgumentNullException.ThrowIfNull(venue);

        await _dbContext.Venues.AddAsync(venue);
        await _dbContext.SaveChangesAsync();

        return venue.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var venue = await _dbContext.Venues.FindAsync(id);

        if (venue is null)
            return null;
        
        _dbContext.Venues.Remove(venue);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(Venue venue)
    {
        ArgumentNullException.ThrowIfNull(venue);

        var venueToUpdate = await _dbContext.Venues
            .FirstOrDefaultAsync(v => v.Id == venue.Id);

        if (venueToUpdate is null)
            return null;

        venueToUpdate.Name = venue.Name;
        venueToUpdate.Latitude = venue.Latitude;
        venueToUpdate.Longtitude = venue.Longtitude;
        venueToUpdate.LogoUrlPath = venue.LogoUrlPath;
        venueToUpdate.Address = venue.Address;
        venueToUpdate.ContactNumber = venue.ContactNumber;
        venueToUpdate.Email = venue.Email;
        venueToUpdate.AveragePrice = venue.AveragePrice;

        await _dbContext.SaveChangesAsync();

        return venueToUpdate.Id;
    }
}
