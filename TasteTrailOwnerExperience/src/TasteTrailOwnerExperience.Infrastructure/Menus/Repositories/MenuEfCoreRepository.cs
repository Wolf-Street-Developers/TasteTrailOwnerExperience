using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Core.Menus.Models;
using TasteTrailOwnerExperience.Core.Menus.Repositories;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

namespace TasteTrailOwnerExperience.Infrastructure.Menus.Repositories;

public class MenuEfCoreRepository : IMenuRepository
{
    private readonly OwnerExperienceDbContext _dbContext;

    public MenuEfCoreRepository(OwnerExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Menu?> GetAsNoTrackingAsync(int id) 
    {
        return await _dbContext.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Menu?> GetByIdAsync(int id)
    {
        return await _dbContext.Menus
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Menu>> GetAllByVenueIdAsync(int venueId)
    {
        return await _dbContext.Menus
            .Where(m => m.VenueId == venueId)
            .ToListAsync();
    }

    public async Task<int> CreateAsync(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        await _dbContext.Menus.AddAsync(menu);
        await _dbContext.SaveChangesAsync();

        return menu.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var menu = await _dbContext.Menus.FindAsync(id);

        if (menu is null)
            return null;
        
        _dbContext.Menus.Remove(menu);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(Menu menu)
    {
        var menuToUpdate = await _dbContext.Menus
            .FirstOrDefaultAsync(m => m.Id == menu.Id);

        if (menuToUpdate is null)
            return null;

        menuToUpdate.Name = menu.Name;
        menuToUpdate.Description = menu.Description;
        menuToUpdate.ImageUrlPath = menu.ImageUrlPath;

        await _dbContext.SaveChangesAsync();

        return menu.Id;
    }
}
