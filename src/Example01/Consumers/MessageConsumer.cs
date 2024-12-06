using Azure.Messaging.ServiceBus;
using Example01.Configuration;
using Example01.Contracts;
using Example01.Extensions;
using Microsoft.Extensions.Options;

namespace Example01.Consumers;

public sealed class MessageConsumer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ServiceBusClient client, IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ServiceBusReceiverOptions
        {
            ReceiveMode = _options.Value.ReceiveMode
        };

        var receiver = _client.CreateReceiver(_options.Value.QueueName, receiverOptions);

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await receiver.ReceiveMessageAsync<Message>(cancellationToken);
            if (message is not null)
            {
                _logger.LogConsumedMessage(message.Id);
            }

            await Task.Delay(_options.Value.ConsumerDelay, cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}