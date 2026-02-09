using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities;

// TODO: to make Role.Name a mandatory field
public class Role : IdentityRole<Guid>
{
    
}