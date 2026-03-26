using Auth.Infrastructure.KafkaTopics;
using Auth.Options;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;

namespace OrderService.API.Bootstrap;

public static class KafkaBootstrap
{
    public static async Task CreateTopics(KafkaOptions options, ILogger logger)
    {
        var adminConfig = new AdminClientConfig()
        {
            BootstrapServers = options.BootstrapServers,
        };
        using var adminClient = new AdminClientBuilder(adminConfig).Build();
            if(await CheckTopicExist(adminClient, logger, EventTopic.Order.ToString())){
                logger.LogInformation("Topic already exists");
                return;
            }
            await adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification()
                {
                    Name = EventTopic.Order.ToString(),
                    NumPartitions = 3,
                    ReplicationFactor = 1
                }
            });
            logger.LogInformation("Topic created");
        
    }

    private static async Task<bool> CheckTopicExist(IAdminClient adminClient, ILogger logger, string topicName)
    {
        try
        {
            var describeResult = await adminClient.DescribeTopicsAsync(
                TopicCollection.OfTopicNames(new[] { EventTopic.Order.ToString() }),
                new DescribeTopicsOptions());
            var topic = describeResult.TopicDescriptions.SingleOrDefault();
            if (topic is not null && topic.Name == EventTopic.Order.ToString())
                return true;
        }
        catch (DescribeTopicsException ex)
        {
            logger.LogWarning(ex.Message, "Failed to describe topics");
        }
        return false;
    }
}