using Microsoft.EntityFrameworkCore;

namespace TasteTrailOwnerExperience.Infrastructure.Common.Data;

public class OwnerExperienceDbContext(DbContextOptions options) : DbContext(options)
{
}
