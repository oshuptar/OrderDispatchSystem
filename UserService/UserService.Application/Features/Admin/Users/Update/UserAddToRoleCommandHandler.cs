using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Features.Admin.Users.Update.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Users.Update;

public class UserAddToRoleCommandHandler(
    IUserRepository userRepository,
    UserManager<User> userManager,
    ILogger<UserAddToRoleCommandHandler> logger
    ) : ICommandHandler<UserAddToRoleRequest>
{
    public async Task HandleCommandAsync(UserAddToRoleRequest command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.Id, cancellationToken);
        if(user is null) throw new NotFoundException("User with id " + command.Id + " not found");
        var res = await userManager.AddToRoleAsync(user, command.Role);
        if(res.Succeeded)
            logger.LogInformation("User {Email} added to role {Role}", user.Email, command.Role);
        else
            logger.LogError("Failed to add user {Email} to role {Role}: {Errors}", user.Email, command.Role, res.Errors.First().Description);
    }
}