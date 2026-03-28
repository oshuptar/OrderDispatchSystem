using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Models;
using OrderService.Application.Models.User;

namespace OrderService.Application.Context;

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