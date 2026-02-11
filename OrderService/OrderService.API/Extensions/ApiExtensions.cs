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