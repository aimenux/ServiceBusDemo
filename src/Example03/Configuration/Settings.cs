using Azure.Messaging.ServiceBus;

namespace Example03.Configuration;

public sealed record Settings
{
    public const string SectionName = "Settings";

    public string QueueName { get; init; } = default!;
    public string ConnectionString { get; init; } = default!;
    public int MaxConcurrentCalls { get; init; } = 1;
    public bool AutoCompleteMessages { get; init; } = true;
    public TimeSpan ConsumerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public TimeSpan ProducerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public ServiceBusTransportType TransportType { get; init; } = ServiceBusTransportType.AmqpWebSockets;
}