using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Abstractions.Authentication;
using UserService.Infrastructure.Options;

namespace UserService.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    public string GenerateToken(GenerateTokenRequest request)
    {
        var (user, userRoles) = request;
        var issuer = jwtOptions.Value.Issuer;
        var audience = jwtOptions.Value.Audience;
        var key = jwtOptions.Value.Key;
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        claims.AddRange(
            userRoles
                .Where(role => !String.IsNullOrWhiteSpace(role))
                .Select(role => new Claim(ClaimTypes.Role, role))
        );
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }
}