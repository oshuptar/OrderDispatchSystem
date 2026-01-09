using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserDb")));
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
}