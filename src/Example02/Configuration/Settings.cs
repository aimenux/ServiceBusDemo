using Azure.Messaging.ServiceBus;

namespace Example02.Configuration;

public sealed record Settings
{
    public const string SectionName = "Settings";

    public string QueueName { get; init; } = default!;
    public string ConnectionString { get; init; } = default!;
    public TimeSpan ConsumerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public TimeSpan ProducerDelay { get; init; } = TimeSpan.FromSeconds(1);
    public ServiceBusReceiveMode ReceiveMode { get; init; } = ServiceBusReceiveMode.ReceiveAndDelete;
    public ServiceBusTransportType TransportType { get; init; } = ServiceBusTransportType.AmqpWebSockets;
}