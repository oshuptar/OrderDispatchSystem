using Auth.Constants;
using Auth.Paths;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Features.Authentication.Login.Contracts;
using UserService.Application.Features.Authentication.Register.Customer.Contracts;
using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
    
namespace UserService.Extensions;

public static class ApiExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        
        // register customer:
        app.MapPost(PathResolver.Auth.Register, async (
            [FromServices] IMediator mediator,
            [FromBody] CustomerRegisterRequest command,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<CustomerRegisterRequest, CustomerRegisterResponse>(command, cancellationToken);
            return Results.Ok(res);
        });
        
        // register worker:
        app.MapPost(PathResolver.Users.Base, async (
            [FromServices] IMediator mediator,
            [FromBody] UserRegisterRequest command,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<UserRegisterRequest, UserRegisterResponse>(command, cancellationToken);
            return Results.Ok(res);
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin, Roles.SuperAdmin]));
        
        // login:
        app.MapPost(PathResolver.Auth.Login, async (
            [FromServices] IMediator mediator,
            [FromBody] UserLoginRequest command,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<UserLoginRequest, UserLoginResponse>(command, cancellationToken);
            return Results.Ok(res);
        });
        
        // get user by id:
        app.MapGet(PathResolver.Users.Base + "/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IMediator mediator, 
            CancellationToken cancellationToken
        ) => {
            var query = new UserGetByIdRequest(id);
            var res = await mediator.ExecuteQueryAsync<UserGetByIdRequest, UserDetailsModel>(
                query,
                cancellationToken);
            return Results.Ok(res);
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin, Roles.SuperAdmin]));
        
        // search users by params
        app.MapGet(PathResolver.Users.Base, async (
            [AsParameters] UserSearchRequest query,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteQueryAsync<UserSearchRequest, UserSearchResponse>(query, cancellationToken);
            return Results.Ok(res);
        }).RequireAuthorization(builder => builder.RequireRole([Roles.Admin, Roles.SuperAdmin]));
        
        app.MapGet("/__claims", (HttpContext ctx) =>
        {
            return ctx.User.Claims.Select(c => new { c.Type, c.Value });
        }).RequireAuthorization();
        
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