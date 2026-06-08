using Auth.Extensions;
using Microsoft.EntityFrameworkCore;
using UserService;
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
    if (app.Environment.IsDevelopment())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    await DataSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}
app.AddMiddleware();
app.MapEndpoints();

app.Run();
