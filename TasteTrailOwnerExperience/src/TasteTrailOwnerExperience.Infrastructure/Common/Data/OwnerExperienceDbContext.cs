using Microsoft.EntityFrameworkCore;
using TasteTrailOwnerExperience.Core.MenuItems.Models;
using TasteTrailOwnerExperience.Core.Menus.Models;
using TasteTrailOwnerExperience.Core.Venues.Models;
using TasteTrailOwnerExperience.Infrastructure.MenuItems.Configurations;
using TasteTrailOwnerExperience.Infrastructure.Menus.Configurations;
using TasteTrailOwnerExperience.Infrastructure.Venues.Configurations;

namespace TasteTrailOwnerExperience.Infrastructure.Common.Data;

public class OwnerExperienceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Menu> Menus { get; set; }

    public DbSet<Venue> Venues { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MenuConfiguration());
        modelBuilder.ApplyConfiguration(new MenuItemConfiguration());
        modelBuilder.ApplyConfiguration(new VenueConfiguration());
    }
}
