namespace ABTesting.Dtos;

public class ExperimentVariantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public string? Content { get; set; }
    public Guid ExperimentId { get; set; }
} 