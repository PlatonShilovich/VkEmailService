using System.ComponentModel.DataAnnotations;

namespace ABTesting.Entities;

public class UserAssignment
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ExperimentId { get; set; }
    public Guid VariantId { get; set; }
    public DateTime AssignedAt { get; set; }
} 