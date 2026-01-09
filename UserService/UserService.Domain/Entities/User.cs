using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities;

// by default, string
public class User : IdentityUser<Guid>
{
    public UserProfile? UserProfile { get; set; }
}