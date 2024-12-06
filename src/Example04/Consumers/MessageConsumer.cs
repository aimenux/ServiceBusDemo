using Example04.Configuration;
using Example04.Contracts;
using Example04.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Example04.Consumers;

public sealed class MessageConsumer : IConsumer<Message>
{
    private readonly Settings _settings;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<Message> context)
    {
        _logger.LogConsumedMessage(context.Message.Id);

        await Task.Delay(_settings.ConsumerDelay, context.CancellationToken);
    }
}