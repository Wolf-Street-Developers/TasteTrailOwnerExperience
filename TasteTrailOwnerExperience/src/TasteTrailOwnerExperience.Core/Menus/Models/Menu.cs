#pragma warning disable CS8618

using System.Text.Json.Serialization;
using TasteTrailOwnerExperience.Core.MenuItems.Models;

namespace TasteTrailOwnerExperience.Core.Menus.Models;

public class Menu
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? ImageUrlPath { get; set; }

    public int VenueId { get; set; }

    public required string UserId { get; set; }

    [JsonIgnore]
    public ICollection<MenuItem> MenuItems { get; set; }
}
