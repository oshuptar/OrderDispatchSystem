using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersBy(UserSearchRequest searchRequest, CancellationToken cancellationToken);
    Task<int> GetUsersByCountAsync(UserSearchRequest searchRequest, CancellationToken cancellationToken);
}
