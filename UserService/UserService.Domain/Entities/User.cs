using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities;

// by default, string
public class User : IdentityUser<int>
{
    public UserProfile UserProfile { get; set; }
}