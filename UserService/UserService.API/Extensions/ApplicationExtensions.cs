using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        return services;
    }
}