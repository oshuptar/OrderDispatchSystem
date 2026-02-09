using Microsoft.AspNetCore.Identity;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.Response;
using UserService.Domain.Entities;

namespace UserService.Application.Mediator.CommandHandlers;

// Imports signInManager. Should define an interface of service in Application and implementation in Infrastructure
// to preserve Clean Architecture?
public class UserLoginCommandHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    ITokenService tokenService
    ) : ICommandHandler<UserLoginRequestDto, UserLoginResponseDto>
{
    public async Task<UserLoginResponseDto> HandleCommandAsync(UserLoginRequestDto command, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(command.Email);
        if (user is null) throw new UnauthorisedException();

        var res = await signInManager.CheckPasswordSignInAsync(user, command.Password, true);
        if (res.RequiresTwoFactor) throw new UnauthorisedException("2FA Required");
        if(res.IsLockedOut) throw new UnauthorisedException("User is locked out");
        if(!res.Succeeded) throw new UnauthorisedException("Not allowed");

        var claims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        
        var accessToken = tokenService.GenerateToken(user, await userManager.GetRolesAsync(user));

        return new UserLoginResponseDto(accessToken);
    }
}