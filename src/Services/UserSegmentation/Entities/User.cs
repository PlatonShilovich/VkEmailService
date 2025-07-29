using System.ComponentModel.DataAnnotations;

namespace UserSegmentation.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Gender { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime RegistrationDate { get; set; }
    public DateTime LastActivity { get; set; }
} 