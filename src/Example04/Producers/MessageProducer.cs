using Example04.Configuration;
using Example04.Contracts;
using Example04.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Example04.Producers;

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
        var sendEndpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
        var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{_settings.QueueName}"));
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = new Message();
            await sendEndpoint.Send(message, cancellationToken);
            _logger.LogProducedMessage(message.Id);
            await Task.Delay(_settings.ProducerDelay, cancellationToken);
        }
    }
}