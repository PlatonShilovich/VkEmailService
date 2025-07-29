namespace Shared.Contracts;

public class Campaign
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid SegmentId { get; set; }
    public Guid ExperimentId { get; set; }
    public string Status { get; set; } = null!;
}