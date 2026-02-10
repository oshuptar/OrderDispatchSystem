using UserService.Application.Abstractions.Authentication;
using UserService.Application.Abstractions.Persistence;
using UserService.Application.Features.Admin.Users.Get;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Features.Admin.Users.Update;
using UserService.Application.Features.Admin.Users.Update.Contracts;
using UserService.Application.Features.Authentication.Login;
using UserService.Application.Features.Authentication.Login.Contracts;
using UserService.Application.Features.Authentication.Register.Customer;
using UserService.Application.Features.Authentication.Register.Customer.Contracts;
using UserService.Application.Features.Authentication.Register.User;
using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Application.Mediator;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Infrastructure.Persistence.UnitOfWork;
using UserService.Infrastructure.Services;

namespace UserService.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICommandHandler<CustomerRegisterRequest, CustomerRegisterResponse>,
            CustomerRegisterCommandHandler>();
        services.AddScoped<ICommandHandler<UserRegisterRequest, UserRegisterResponse>, 
            UserRegisterCommandHandler>();
        services.AddScoped<ICommandHandler<UserLoginRequest, UserLoginResponse>,
            UserLoginCommandHandler>();
        services.AddScoped<IQueryHandler<UserGetByIdRequest, UserModel>,
            UserGetByIdQueryHandler>();
        services.AddScoped<ICommandHandler<UserAddToRoleRequest>, UserAddToRoleCommandHandler>();
        services.AddScoped<ITokenService, JwtTokenService>();
        return services;
    }
}