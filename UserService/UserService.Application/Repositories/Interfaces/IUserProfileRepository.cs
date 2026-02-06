using UserService.Domain.Entities;

namespace UserService.Application.Repositories.Interfaces;

public interface IUserProfileRepository
{
    Task CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken);
}