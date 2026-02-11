using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(
    UserDbContext userDbContext
    ) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await userDbContext.Users.Include(user => user.UserProfile)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetUsersAsync(UserSearchRequestModel searchRequest, CancellationToken cancellationToken)
    {
        return await userDbContext.Users
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
            .Where(res => searchRequest.Role == null 
                          || res.RoleName == searchRequest.Role.ToUpper())
            .Where(res => searchRequest.Email == null 
                          || res.User.Email == searchRequest.Email)
            .Select(res => res.User)
            .Distinct()
            .OrderBy(u => u.Id)
            .Skip((searchRequest.Page - 1) * searchRequest.Size)
            .Take(searchRequest.Size)
            .Include(user => user.UserProfile)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUsersCountAsync(UserSearchRequest searchRequest, CancellationToken cancellationToken)
    {
        return await userDbContext.Users
            .Join(userDbContext.UserRoles,
                user => user.Id,
                roles => roles.UserId,
                (user, roles) => new { User = user, Roles = roles }
            )
            .Join(userDbContext.Roles,
                userRoles => userRoles.Roles.RoleId,
                roles => roles.Id,
                (userRoles, roles) => new { User = userRoles.User, RoleName = roles.NormalizedName }
            )
            .Where(res => searchRequest.Role == null
                          || res.RoleName == searchRequest.Role.ToUpper())
            .Where(res => searchRequest.Email == null
                          || res.User.Email == searchRequest.Email)
            .Select(res => res.User)
            .Distinct()
            .CountAsync(cancellationToken);
    }
}