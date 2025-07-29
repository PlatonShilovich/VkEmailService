namespace Analytics.Dtos;

public class EventDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CampaignId { get; set; }
    public string EventType { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public string Metadata { get; set; } = null!;
} 