using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using UserService;
using UserService.Domain.Entities;
using UserService.Extensions;
using UserService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUserServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Used for seeding data
using (var scope = app.Services.CreateScope()){
    await DataSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}
app.AddMiddleware();
app.MapEndpoints();

app.Run();
