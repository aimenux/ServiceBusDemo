using Example07.Configuration;
using Example07.Contracts;
using Example07.Extensions;
using Microsoft.Extensions.Options;
using SlimMessageBus;

namespace Example07.Producers;

public sealed class MessageProducer : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly Settings _settings;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IMessageBus bus, IOptions<Settings> options, ILogger<MessageProducer> logger)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            await _bus.Publish(message, cancellationToken: cancellationToken);
            _logger.LogProducedMessage(message.Id);
            await Task.Delay(_settings.ProducerDelay, cancellationToken);
        }
    }
}