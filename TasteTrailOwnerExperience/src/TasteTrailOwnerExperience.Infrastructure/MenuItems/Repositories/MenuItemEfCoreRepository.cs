using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Core.MenuItems.Models;
using TasteTrailOwnerExperience.Core.MenuItems.Repositories;
using TasteTrailOwnerExperience.Infrastructure.Common.Data;

namespace TasteTrailOwnerExperience.Infrastructure.MenuItems.Repositories;

public class MenuItemEfCoreRepository : IMenuItemRepository
{
    private readonly OwnerExperienceDbContext _dbContext;

    public MenuItemEfCoreRepository(OwnerExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<MenuItem?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }

    public async Task<IEnumerable<MenuItem>> GetAllByMenuIdAsync(int menuId)
    {
        return await _dbContext.MenuItems
            .Where(m => m.MenuId == menuId)
            .ToListAsync();
    }

    public async Task<int> CreateAsync(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        var menu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == menuItem.MenuId) ?? 
            throw new ArgumentException($"Menu by ID: {menuItem.MenuId} not found.");

        await _dbContext.MenuItems.AddAsync(menuItem);
        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var menuItem = await _dbContext.MenuItems.FindAsync(id);

        if (menuItem is null)
            return null;
        
        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(MenuItem menuItem)
    {
        var menuItemToUpdate = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == menuItem.Id);

        if (menuItemToUpdate is null)
            return null;
        
        menuItemToUpdate.Name = menuItem.Name;
        menuItemToUpdate.Description = menuItem.Description;
        menuItemToUpdate.Price = menuItem.Price;

        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }
}
