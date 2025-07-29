using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserSegmentation.Entities;

public class Segment
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Required]
    public string Criteria { get; set; } = null!; // JSON
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 