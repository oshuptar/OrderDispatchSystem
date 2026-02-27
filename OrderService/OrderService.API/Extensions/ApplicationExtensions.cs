using Auth.Abstractions.Persistence;
using Auth.Mediator;
using Auth.Mediator.Interfaces;
using Auth.Persistence.UnitOfWork;
using OrderService.Application.Features.Address.Get;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Application.Features.OrderPlatform.OrderCreate;
using OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;
using OrderService.Infrastructure.Persistence;

namespace OrderService.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IUnitOfWork, UnitOfWork<OrderDbContext>>();
        
        // Command Handlers:
        services.AddScoped<ICommandHandler
            <ClientCreateOrderRequestModel, ClientCreateOrderResponseModel>,
            ClientOrderCreateCommandHandler>();
            
        // Query Handlers:
        services.AddScoped<IQueryHandler
            <AddressSearchRequest, AddressSearchResponse>,
            AddressSearchByQueryHandler>();
        
        return services;
    }
}