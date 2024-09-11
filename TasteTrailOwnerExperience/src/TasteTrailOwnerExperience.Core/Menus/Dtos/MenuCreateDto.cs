namespace TasteTrailOwnerExperience.Core.Menus.Dtos;

public class MenuCreateDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public int VenueId { get; set; }
}
