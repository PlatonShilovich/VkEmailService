namespace Shared.Contracts;

public class ExperimentVariant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public string Content { get; set; } = null!;
    public Guid ExperimentId { get; set; }
}