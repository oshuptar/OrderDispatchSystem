using Auth.Extensions;
using OrderService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOptions();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOrderServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

app.AddMiddleware();
app.MapEndpoints();

app.Run();
