using Auth.Constants;
using Auth.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

namespace OrderService.API.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(Databases.Order.DbName)));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDeliveryRepository, OrderDeliveryRepository>();
        services.AddScoped<IProductionPlantRepository, ProductionPlantRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtOptions(configuration);
        return services;
    }
}