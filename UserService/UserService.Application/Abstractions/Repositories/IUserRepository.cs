using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAsync(UserSearchRequestModel searchRequest, CancellationToken cancellationToken);
    Task<int> GetCountAsync(UserSearchRequest searchRequest, CancellationToken cancellationToken);
}