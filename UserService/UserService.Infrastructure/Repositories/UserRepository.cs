using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(
    UserDbContext userDbContext
    ) : IUserRepository
{
    public IQueryable<User> GetUserByIdAsync(Guid userId) => userDbContext.Users.Where(user => user.Id == userId);
    
    
}