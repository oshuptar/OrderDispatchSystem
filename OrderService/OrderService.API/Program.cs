using Auth.Extensions;
using Auth.Options;
using Microsoft.Extensions.Options;
using OrderService.API.Bootstrap;
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
try
{
    using var scope = app.Services.CreateScope();
    await KafkaBootstrap.CreateTopics(
        builder.Configuration.GetSection(KafkaOptions.SectionName).Get<KafkaOptions>() ?? throw new Exception("Invalid Kafka Options"),
        app.Logger);
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Kafka topic bootstrap failed.");
}
app.AddMiddleware();
app.MapEndpoints();
app.Run();
