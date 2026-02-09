using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    public String GenerateToken(User user, IEnumerable<String> userRoles);
}