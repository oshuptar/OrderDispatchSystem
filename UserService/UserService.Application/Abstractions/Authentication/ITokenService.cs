using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Authentication;

public record GenerateTokenRequest(User User, IEnumerable<String> UserRoles);

public interface ITokenService
{
    public String GenerateToken(GenerateTokenRequest request);
}