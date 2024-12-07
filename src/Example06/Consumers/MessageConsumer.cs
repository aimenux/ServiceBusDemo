using Example06.Configuration;
using Example06.Contracts;
using Example06.Extensions;
using Microsoft.Extensions.Options;
using Rebus.Handlers;

namespace Example06.Consumers;

public sealed class MessageConsumer : IHandleMessages<Message>
{
    private readonly Settings _settings;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(Message message)
    {
        _logger.LogConsumedMessage(message.Id);

        await Task.Delay(_settings.ConsumerDelay);
    }
}