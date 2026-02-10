using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Common.Exceptions;
using UserService.Application.Common.Mappers;
using UserService.Application.Features.Admin.Users.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Users;

public class UserGetByIdQueryHandler(
    IUserRepository userRepository,
    UserManager<User> userManager
    ) : IQueryHandler<UserGetByIdRequest, UserModel>
{
    public async Task<UserModel> HandleQueryAsync(UserGetByIdRequest command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetUserByIdAsync(command.Id)
            .Include(user => user.UserProfile)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new NotFoundException("User with id " + command.Id + " not found");
        var roles = await userManager.GetRolesAsync(user);
        return user.ToUserModel(new UserModelContext(roles));
    }
}