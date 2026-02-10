using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using UserService.Application.Features.Admin.Users.Contracts;
using UserService.Application.Features.Authentication.Login.Contracts;
using UserService.Application.Features.Authentication.Register.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Constants;
    
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
        
        // /register:
        app.MapPost(PathResolver.Auth.Register, async ([FromServices] IMediator mediator,
            [FromBody] UserRegisterRequest command,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<UserRegisterRequest, UserRegisterResponse>(command, cancellationToken);
            return Results.Ok(res);
        });
        
        // /login:
        app.MapPost(PathResolver.Auth.Login, async ([FromServices] IMediator mediator,
            [FromBody] UserLoginRequest command,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<UserLoginRequest, UserLoginResponse>(command, cancellationToken);
            return Results.Ok(res);
        });
        
        app.MapGet(PathResolver.Users.Base + "/{id:guid}", async (
            [FromRoute]Guid id,
            [FromServices] IMediator mediator, 
            CancellationToken cancellationToken
        ) => {
            var res = await mediator.ExecuteQueryAsync<UserGetByIdRequest, UserModel>(new UserGetByIdRequest(id),
                cancellationToken);
            return Results.Ok(res);
        }).RequireAuthorization(builder => builder.RequireRole(Roles.Admin));
        
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