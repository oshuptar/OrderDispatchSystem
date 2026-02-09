using Microsoft.AspNetCore.Identity;
using UserService.Application.Interfaces;
using UserService.Application.Mediator;
using UserService.Application.Mediator.CommandHandlers;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.Response;
using UserService.Application.UnitOfWork.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Services;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICommandHandler<UserRegisterRequestDto, UserRegisterResponseDto>,
            UserRegisterCommandHandler>();
        services.AddScoped<ICommandHandler<UserLoginRequestDto, UserLoginResponseDto>,
            UserLoginCommandHandler>();
        services.AddScoped<ITokenService, JwtTokenService>();
        return services;
    }
}