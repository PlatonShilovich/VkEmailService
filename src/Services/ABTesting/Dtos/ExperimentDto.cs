namespace ABTesting.Dtos;

public class ExperimentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string TrafficSplit { get; set; } = null!;
    public List<ExperimentVariantDto> Variants { get; set; } = new();
} 