using Scalar.AspNetCore;
using UserService.Domain.Entities;

namespace UserService.Extensions;

public static class ApiExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapIdentityApi<User>();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
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