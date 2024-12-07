using Azure.Messaging.ServiceBus;

namespace Example06.Configuration;

public sealed record Settings
{
    public const string SectionName = "Settings";

    public string QueueName { get; init; } = default!;
    public string ConnectionString { get; init; } = default!;
    public RetrySettings RetrySettings { get; init; } = new();
    public TimeSpan ConsumerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public TimeSpan ProducerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public ServiceBusTransportType TransportType { get; init; } = ServiceBusTransportType.AmqpWebSockets;
}

public sealed record RetrySettings
{
    public int RetryCount { get; init; } = 3;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(5);
}