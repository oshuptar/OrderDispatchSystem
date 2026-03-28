using Auth.Abstractions.Persistence;
using Auth.Mediator;
using Auth.Mediator.Interfaces;
using Auth.Persistence.UnitOfWork;
using OrderService.Application.Context;
using OrderService.Application.Features.Order.Create;
using OrderService.Application.Workers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adds the possibility to inject IHttpContextAccessor
        services.AddHttpContextAccessor();
        services.AddScoped<UserContext>();
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork<OrderDbContext>>();
        
        // Command Handlers:
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(ICommandHandler<,>));
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(ICommandHandler<>));
        // Query Handlers:
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(IQueryHandler<,>));
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(IQueryHandler<>));
        
        // Background workers:
        // No scope is created for a hosted service by default
        services.AddHostedService<OutboxPublisherWorker>();
        return services;
    }

    public static IServiceCollection AddServiceImplementation(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Type genericType)
    {
        var implementations = typeof(OrderCreateCommandHandler).Assembly
            .GetTypes()
            .Where(type => !type.IsGenericType && !type.IsAbstract && type.GetInterfaces().Any(interfaceType =>
                interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType));
        foreach (Type implementation in implementations)
        {  
            var interfaceTypes= implementation.GetInterfaces()
                .Where(interfaceType =>
                    interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType);
            foreach(var interfaceType in interfaceTypes)
                services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }
        return services;
    }
}