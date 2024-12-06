using Example05.Configuration;
using Example05.Contracts;
using Example05.Extensions;
using Microsoft.Extensions.Options;
using Wolverine;

namespace Example05.Producers;

public sealed class MessageProducer : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IServiceScopeFactory factory, IOptions<Settings> options, ILogger<MessageProducer> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _factory.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            await bus.SendAsync(message);
            _logger.LogProducedMessage(message.Id);
            await Task.Delay(_options.Value.ProducerDelay, cancellationToken);
        }
    }
}