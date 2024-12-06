using Azure.Messaging.ServiceBus;
using Example03.Configuration;
using Example03.Contracts;
using Example03.Extensions;
using Microsoft.Extensions.Options;

namespace Example03.Consumers;

public sealed class MessageConsumer : BackgroundService, IAsyncDisposable
{
    private readonly ServiceBusProcessor _processor;
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ServiceBusProcessor processor, IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _processor.StartProcessingAsync(cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _processor.DisposeAsync();
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body;
        var message = body?.ToObjectFromJson<Message>();
        if (message is not null)
        {
            _logger.LogConsumedMessage(message.Id);
        }

        await Task.Delay(_options.Value.ConsumerDelay, args.CancellationToken);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        var messageInfo = new
        {
            args.Identifier,
            args.EntityPath,
            args.ErrorSource,
            args.Exception
        };

        _logger.LogFailedMessage(messageInfo);

        return Task.CompletedTask;
    }
}