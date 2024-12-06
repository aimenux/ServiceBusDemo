using Azure.Messaging.ServiceBus;

namespace Example01.Extensions;

public static class ServiceBusExtensions
{
    private static TimeSpan MaxWaitTime => TimeSpan.FromSeconds(5);

    public static async Task<T?> ReceiveMessageAsync<T>(this ServiceBusReceiver receiver, CancellationToken cancellationToken) where T : class
    {
        var serviceBusMessage = await receiver.ReceiveMessageAsync(MaxWaitTime, cancellationToken);
        var message = serviceBusMessage?.Body?.ToObjectFromJson<T>();
        return message;
    }
}