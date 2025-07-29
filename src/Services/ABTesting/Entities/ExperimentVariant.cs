using System.ComponentModel.DataAnnotations;

namespace ABTesting.Entities;

public class ExperimentVariant
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public string? Content { get; set; }
    public Guid ExperimentId { get; set; }
    public Experiment Experiment { get; set; } = null!;
} 