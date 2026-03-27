using Auth.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KafkaOptions>()
            .Bind(configuration.GetSection(KafkaOptions.SectionName))
            .Validate(option => !string.IsNullOrWhiteSpace(option.BootstrapServers),
                    $"{KafkaOptions.SectionName}:{nameof(KafkaOptions.BootstrapServers)} is missing")
            .ValidateOnStart();
        return services;
    }
}