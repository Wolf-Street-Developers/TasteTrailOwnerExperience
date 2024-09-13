namespace TasteTrailOwnerExperience.Infrastructure.Common.RabbitMq.Dtos;

public class ImageMessageDto
{
    public required string ImageUrl { get; set; }

    public required int EntityId { get; set; }
}
