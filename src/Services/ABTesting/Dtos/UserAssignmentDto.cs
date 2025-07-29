namespace ABTesting.Dtos;

public class UserAssignmentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ExperimentId { get; set; }
    public Guid VariantId { get; set; }
    public DateTime AssignedAt { get; set; }
} 