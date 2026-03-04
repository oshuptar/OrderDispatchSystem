using Auth.Constants;
using Auth.Mediator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.OrderPlatform.PostOrder.Contracts;
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