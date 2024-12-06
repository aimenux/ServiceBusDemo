using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Example02.Configuration;
using Example02.Contracts;
using Example02.Extensions;
using Microsoft.Extensions.Options;

namespace Example02.Producers;

public sealed class MessageProducer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusSender _sender;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(ServiceBusSender sender, IOptions<Settings> options, ILogger<MessageProducer> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));
            await _sender.SendMessageAsync(serviceBusMessage, cancellationToken);
            _logger.LogProducedMessage(message.Id);
            await Task.Delay(_options.Value.ProducerDelay, cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
    }
}