using Auth.Extensions;
using DriverService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDriverServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
var app = builder.Build();
app.AddMiddleware();
app.MapEndpoints();
app.Run();