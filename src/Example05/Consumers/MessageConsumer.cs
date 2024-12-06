using Example05.Configuration;
using Example05.Contracts;
using Example05.Extensions;
using Microsoft.Extensions.Options;

namespace Example05.Consumers;

public sealed class MessageConsumer
{
    private readonly IOptions<Settings> _options;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(Message message)
    {
        _logger.LogConsumedMessage(message.Id);

        await Task.Delay(_options.Value.ConsumerDelay);
    }
}