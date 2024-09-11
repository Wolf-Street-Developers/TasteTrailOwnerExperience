namespace TasteTrailOwnerExperience.Core.MenuItems.Dtos;

public class MenuItemUpdateDto
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public float Price { get; set; }
}
