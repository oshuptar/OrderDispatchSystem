using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserProfileRepository
{
    Task CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken);
}