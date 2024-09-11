using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailOwnerExperience.Core.Venues.Models;

namespace TasteTrailOwnerExperience.Infrastructure.Venues.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(v => v.Description)
            .HasMaxLength(1000);

        builder.Property(v => v.ContactNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(v => v.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.AveragePrice)
            .IsRequired();

        builder.Property(v => v.Rating)
            .IsRequired()
            .HasPrecision(7, 3);

        builder.Property(v => v.CreationDate)
            .IsRequired();

        builder.Property(v => v.Longtitude)
            .IsRequired();

        builder.Property(v => v.Latitude)
            .IsRequired();

        builder.HasMany(v => v.Menus)
            .WithOne()
            .HasForeignKey(m => m.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}