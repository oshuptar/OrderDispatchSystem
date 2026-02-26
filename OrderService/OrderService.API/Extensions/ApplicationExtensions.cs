using Auth.Abstractions.Persistence;
using Auth.Mediator;
using Auth.Mediator.Interfaces;
using Auth.Persistence.UnitOfWork;
using OrderService.Infrastructure.Persistence;

namespace OrderService.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork<OrderDbContext>>();

        return services;
    }
}