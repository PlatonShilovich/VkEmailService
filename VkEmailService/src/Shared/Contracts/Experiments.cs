namespace Shared.Contracts;

public class Experiment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string TrafficSplit { get; set; } = null!; // JSON
    public List<ExperimentVariant> Variants { get; set; } = new();
}