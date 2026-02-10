using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Common.Exceptions;
using UserService.Application.Common.Mappers;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Users.Get;

public class UserGetByIdQueryHandler(
    IUserRepository userRepository,
    UserManager<User> userManager
    ) : IQueryHandler<UserGetByIdRequest, UserDetailsModel>
{
    public async Task<UserDetailsModel> HandleQueryAsync(UserGetByIdRequest command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetUserByIdAsync(command.Id, cancellationToken);
        if (user is null) throw new NotFoundException("User with id " + command.Id + " not found");
        var roles = await userManager.GetRolesAsync(user);
        return user.ToUserDetailsModel(new UserModelContext(roles));
    }
}