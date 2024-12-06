using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Example03.Configuration;
using Example03.Contracts;
using Microsoft.Extensions.Options;
using LoggingExtensions = Example03.Extensions.LoggingExtensions;

namespace Example03.Producers;

public sealed class MessageProducer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(ServiceBusClient client, IOptions<Settings> options, ILogger<MessageProducer> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var sender = _client.CreateSender(_options.Value.QueueName);

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));
            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
            LoggingExtensions.LogProducedMessage(_logger, message.Id);
            await Task.Delay(_options.Value.ProducerDelay, cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}