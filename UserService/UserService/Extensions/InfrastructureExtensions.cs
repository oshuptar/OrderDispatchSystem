using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories.Interfaces;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserDb")));
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}