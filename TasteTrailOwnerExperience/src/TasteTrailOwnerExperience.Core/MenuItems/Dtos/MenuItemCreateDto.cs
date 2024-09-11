namespace TasteTrailOwnerExperience.Core.MenuItems.Dtos;

public class MenuItemCreateDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public float Price { get; set; }

    public int MenuId { get; set; }
}
