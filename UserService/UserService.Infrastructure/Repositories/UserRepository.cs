using System.Linq.Expressions;
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
    
    // Better than userManager.getInRoleAsync, since it returns IQueryable
    public IQueryable<User> GetUsersInRole(String role)
    {
        return userDbContext.Users
            .Join(userDbContext.UserRoles,
                user  => user.Id,
                roles => roles.UserId,
                (user, roles) => new { User = user, Roles = roles }
            )
            .Join(userDbContext.Roles,
                userRoles => userRoles.Roles.RoleId,
                roles => roles.Id,
                (userRoles, roles) => new { User = userRoles.User, RoleName = roles.NormalizedName }
            )
            .Where(res => res.RoleName == role.ToUpper())
            .Select(res => res.User)
            .Distinct()
            .OrderBy(u => u.Id);
    }

    public IQueryable<User> GetAllUsers() => userDbContext.Users;
}