using System.ComponentModel.DataAnnotations;

namespace ABTesting.Entities;

public class Experiment
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string TrafficSplit { get; set; } = null!; // JSON
    public ICollection<ExperimentVariant> Variants { get; set; } = new List<ExperimentVariant>();
} 