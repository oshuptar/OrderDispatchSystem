using Auth.Exceptions;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions.Authentication;
using UserService.Application.Features.Authentication.Login.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Login;

public class UserLoginCommandHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    ITokenService tokenService
    ) : ICommandHandler<UserLoginRequest, UserLoginResponse>
{
    public async Task<UserLoginResponse> HandleCommandAsync(UserLoginRequest command, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(command.Email);
        if (user is null) throw new UnauthorisedException();

        var res = await signInManager.CheckPasswordSignInAsync(user, command.Password, true);
        if (res.RequiresTwoFactor) throw new UnauthorisedException("2FA Required");
        if(res.IsLockedOut) throw new UnauthorisedException("User is locked out");
        if(!res.Succeeded) throw new UnauthorisedException("Not allowed");

        var claims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        
        var accessToken = tokenService.GenerateToken(new GenerateTokenRequest(user, await userManager.GetRolesAsync(user)));

        return new UserLoginResponse(accessToken);
    }
}