using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Context;

public class UserContext
{
    private readonly IHttpContextAccessor _accessor;
    public Guid UserId { get; }
    public String Email { get; }
    
    public UserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        // TODO: make ClaimTypes consistent with JWT Token generation. ClaimTypes.NameIdentifier is different from JwtRegisteredClaims.Sub
        if(Guid.TryParse(_accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            UserId = userId;
        else
            throw new Exception($"Failed to parse registered claim: {nameof(ClaimTypes.NameIdentifier)}");
        String? email = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        if (email != null)
            Email = email;
        else
            throw new Exception($"Failed to  parse claim: {nameof(JwtRegisteredClaimNames.Email)}");
    }
}