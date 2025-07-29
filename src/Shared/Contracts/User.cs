namespace Shared.Contracts;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Gender { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime RegistrationDate { get; set; }
    public DateTime LastActivity { get; set; }
}

