using Example06.Configuration;
using Example06.Contracts;
using Example06.Extensions;
using Microsoft.Extensions.Options;
using Rebus.Bus;

namespace Example06.Producers;

public sealed class MessageProducer : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly Settings _settings;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IServiceScopeFactory factory, IOptions<Settings> options, ILogger<MessageProducer> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _factory.CreateScope();
        using var bus = scope.ServiceProvider.GetRequiredService<IBus>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            await bus.Send(message);
            _logger.LogProducedMessage(message.Id);
            await Task.Delay(_settings.ProducerDelay, cancellationToken);
        }
    }
}