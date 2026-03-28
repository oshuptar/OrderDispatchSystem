using Auth.Constants;
using Auth.Mediator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Admin.OrderAssignProductionPlantCommandHandler.Contracts;
using OrderService.Application.Features.Admin.OrderFinalizeVerification.Contracts;
using OrderService.Application.Features.Admin.OrderStartVerification.Contracts;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.OrderPlatform.PostOrder.Contracts;
using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;
using OrderService.Application.Models.Order.In;
using Scalar.AspNetCore;

namespace OrderService.API.Extensions;

public static class ApiExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        
        // Post an order by client from order platform
        app.MapPost (PathResolver.Orders.Base, async (
                [FromServices] IMediator mediator,
                [FromBody] ClientCreateOrderRequest command,
                CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<ClientCreateOrderRequest, ClientCreateOrderResponse>(command, cancellationToken);
            return Results.Ok(res);
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Customer]));
        
        // Start order verification by admin
        app.MapPost(PathResolver.Orders.StartVerification, async (
            [FromRoute] Guid orderId,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            await mediator.ExecuteCommandAsync<OrderStartVerificationRequest>(new OrderStartVerificationRequest(orderId), cancellationToken);
            return Results.Ok();
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin]));
        
        // Update order by client before verification starts
        app.MapPatch(PathResolver.Orders.ById, async (
            [FromRoute] Guid orderId,
            [FromServices] IMediator mediator,
            [FromBody] ClientOrderUpdateInputRequestModel inputRequestModel,  
            CancellationToken cancellationToken
        ) =>
        {
            var command = new ClientOrderUpdateRequest(orderId, inputRequestModel.Volume, inputRequestModel.Weight,
                inputRequestModel.RequestedDeliveryDateTime, inputRequestModel.DestionationAddressUpdateRequest);
            await mediator.ExecuteCommandAsync<ClientOrderUpdateRequest>(command, cancellationToken);
            return Results.Ok();
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Customer]));
        
        // Finalize verification process. Any further changes to the order are not allowed, except dispatching responsibilities
        app.MapPost(PathResolver.Orders.ById, async (
            [FromRoute] Guid orderId,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) =>
        {
            await mediator.ExecuteCommandAsync<OrderFinalizeVerificationRequest>(new OrderFinalizeVerificationRequest(orderId), cancellationToken);
            return Results.Ok();
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin]));
        
        // Potentially there would be a separate dispatcher to delegate orders to specific production plants based on metrics. For now, this is admin's responsibility
        // Assigns a ProductionPlantId to the order
        app.MapPost(PathResolver.Orders.ById, async (
            [FromRoute] Guid orderId,
            [FromServices] IMediator mediator,
            [FromBody] OrderAssignProductionPlantInputRequestModel requestModel,
            CancellationToken cancellationToken
            ) =>
        {
            await mediator.ExecuteCommandAsync<OrderAssignProductionPlantRequest>(new OrderAssignProductionPlantRequest(orderId, requestModel.ProductionPlantId), cancellationToken);
            return Results.Ok();
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin]));
        
        // Search orders by parameters
        app.MapGet(PathResolver.Orders.Base, async (
            [FromQuery] OrderSearchByRequest queryParams,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
            ) =>
        {
            var res = await mediator.ExecuteQueryAsync<OrderSearchByRequest, OrderSearchByResponse>(queryParams, cancellationToken);
            return Results.Ok(res);
        });
        
        return app;
    }
    
    public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}