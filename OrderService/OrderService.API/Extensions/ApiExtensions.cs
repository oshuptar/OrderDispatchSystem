using Auth.Constants;
using Auth.Mediator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Admin.OrderStartVerification.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Features.OrderPlatform.PostOrder.Contracts;
using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;
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
        });
        
        // Update order by client before verification starts
        app.MapPatch(PathResolver.Orders.ById, async (
            [FromRoute] Guid orderId,
            [FromServices] IMediator mediator,
            [FromBody] ClientOrderUpdateRequest command,  
            CancellationToken cancellationToken
        ) =>
        {
            await mediator.ExecuteCommandAsync<ClientOrderUpdateRequest>(command, cancellationToken);
            return Results.Ok();
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