namespace UserService.Domain.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public int UserId { get; set; }
    public User? User { get; set; }
}