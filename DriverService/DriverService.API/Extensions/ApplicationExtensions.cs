using Auth.Abstractions.Persistence;
using Auth.Context;
using Auth.Mediator;
using Auth.Mediator.Interfaces;
using Auth.Persistence.UnitOfWork;
using DriverService.Infrastructure.Persistence;

namespace DriverService.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddDriverServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UserContext>();
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork<DriverDbContext>>();
        
        // Command Handlers:
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(ICommandHandler<,>));
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(ICommandHandler<>));
        // Query Handlers:
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(IQueryHandler<,>));
        services.AddServiceImplementation(ServiceLifetime.Scoped, typeof(IQueryHandler<>));
        return services;
    }
    
    public static IServiceCollection AddServiceImplementation(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Type genericType)
    { 
        // TODO: uncoment when at least one service is registered
    //     var implementations = typeof().Assembly
    //         .GetTypes()
    //         .Where(type => !type.IsGenericType && !type.IsAbstract && type.GetInterfaces().Any(interfaceType =>
    //             interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType));
    //     foreach (Type implementation in implementations)
    //     {  
    //         var interfaceTypes= implementation.GetInterfaces()
    //             .Where(interfaceType =>
    //                 interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType);
    //         foreach(var interfaceType in interfaceTypes)
    //             services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
    //     }
        return services;
    }
}