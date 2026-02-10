using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities;

// TODO: to make email and username mandatory fields
public class User : IdentityUser<Guid>
{
    public UserProfile? UserProfile { get; set; }
}