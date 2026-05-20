using Auth.Constants;
using Auth.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.EventStreaming.EventStreaming;
using OrderService.Infrastructure.Interceptors;
using OrderService.Infrastructure.Kafka;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

namespace OrderService.API.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptor>();
        services.AddDbContext<OrderDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(Databases.Order.DbName));
            options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
        });

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDeliveryRepository, OrderDeliveryRepository>();
        services.AddScoped<IProductionPlantRepository, ProductionPlantRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

        // Registers Kafka Event Producer
        services.AddSingleton<IEventProducer, KafkaEventProducer>();
        return services;
    }
    
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtOptions(configuration);
        services.AddKafkaOptions(configuration);
        return services;
    }
}
