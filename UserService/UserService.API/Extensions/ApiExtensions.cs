using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.Response;

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
            [FromBody] UserRegisterRequestDto dto,
            CancellationToken cancellationToken) =>
        {
            var res = await mediator.ExecuteCommandAsync<UserRegisterRequestDto, UserRegisterResponseDto>(dto, cancellationToken);
            return Results.Ok(res);
        });
        
        // /login
        app.MapPost(PathResolver.Auth.Login, async ([FromServices] IMediator mediator,
            [FromBody] UserLoginRequestDto dto,
            CancellationToken cancellationToken) =>
        {
            await mediator.ExecuteCommandAsync(dto, cancellationToken);
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