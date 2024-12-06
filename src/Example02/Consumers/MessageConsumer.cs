using Azure.Messaging.ServiceBus;
using Example02.Configuration;
using Example02.Contracts;
using Example02.Extensions;
using Microsoft.Extensions.Options;

namespace Example02.Consumers;

public sealed class MessageConsumer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusReceiver _receiver;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ServiceBusReceiver receiver, IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await _receiver.ReceiveMessageAsync<Message>(cancellationToken);
            if (message is not null)
            {
                _logger.LogConsumedMessage(message.Id);
            }

            await Task.Delay(_options.Value.ConsumerDelay, cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _receiver.DisposeAsync();
    }
}