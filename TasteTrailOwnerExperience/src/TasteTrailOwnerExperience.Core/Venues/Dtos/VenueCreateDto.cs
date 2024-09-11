namespace TasteTrailOwnerExperience.Core.Venues.Dtos;

public class VenueCreateDto
{
    public required string Name { get; set; }

    public required string Address { get; set; }

    public string? Description { get; set; }
    
    public required string Email { get; set; }

    public string? ContactNumber { get; set; }

    public float AveragePrice { get; set; }
    
    public double Longtitude { get; set; }

    public double Latitude { get; set; }
}
