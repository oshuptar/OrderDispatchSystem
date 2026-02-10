using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserRepository
{
    IQueryable<User> GetUserByIdAsync(Guid userId);
}