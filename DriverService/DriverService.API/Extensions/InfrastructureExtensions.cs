using Auth.Constants;
using Auth.Extensions;
using DriverService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DriverService.API.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DriverDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(Databases.Driver.DbName));
        });
        return services;
    }
    
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtOptions(configuration);
        services.AddKafkaOptions(configuration);
        return services;
    }
}