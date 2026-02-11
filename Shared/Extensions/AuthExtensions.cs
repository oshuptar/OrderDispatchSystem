using System.Text;
using Auth.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(option => !string.IsNullOrWhiteSpace(option.Issuer),
                $"{JwtOptions.SectionName}:{nameof(JwtOptions.Issuer)} is missing")
            .Validate(option => !string.IsNullOrWhiteSpace(option.Audience),
                $"{JwtOptions.SectionName}:{nameof(JwtOptions.Audience)} is missing")
            .Validate(option => !string.IsNullOrWhiteSpace(option.Key),
                $"{JwtOptions.SectionName}:{nameof(JwtOptions.Key)} is missing")
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() 
                  ?? throw new Exception("Invalid Jwt Options");
        services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        return services;
    }
}