using System.Security.Claims;
using Auth.Context.Models;
using Microsoft.AspNetCore.Http;

namespace Auth.Context;

public class UserContext(IHttpContextAccessor accessor)
{
    public UserModel? User
    {
        get
        {
            if (Guid.TryParse(accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
                return new UserModel(userId);
            return null;
        }
    }
}