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

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            // options.Password.RequireDigit = true;
            // options.Password.RequireLowercase = true;
            // options.Password.RequireNonAlphanumeric = true;
            // options.Password.RequireUppercase = true;
            // options.Password.RequiredLength = 10;
            //
            // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            // options.Lockout.MaxFailedAccessAttempts = 5;
            // options.Lockout.AllowedForNewUsers = true;
            //
            // options.User.RequireUniqueEmail = true;
            //
            // options.SignIn.RequireConfirmedEmail = true;
        });
        return services;
    }
}