using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserProfileRepository(UserDbContext userDbContext) : IUserProfileRepository
{
    public Task CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken)
    {
        userDbContext.UserProfiles.Add(userProfile);
        return Task.CompletedTask;
    }
}