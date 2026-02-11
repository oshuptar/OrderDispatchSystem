using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersAsync(UserSearchRequest searchRequest, CancellationToken cancellationToken);
    Task<int> GetUsersCountAsync(UserSearchRequest searchRequest, CancellationToken cancellationToken);
}
