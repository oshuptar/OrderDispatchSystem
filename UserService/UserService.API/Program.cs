using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using UserService.Domain.Entities;
using UserService.Extensions;
using UserService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddUserServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddMiddleware();
app.MapEndpoints();

app.Run();