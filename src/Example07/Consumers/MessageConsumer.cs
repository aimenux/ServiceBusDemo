using Example07.Configuration;
using Example07.Contracts;
using Example07.Extensions;
using Microsoft.Extensions.Options;
using SlimMessageBus;

namespace Example07.Consumers;

public sealed class MessageConsumer : IConsumer<Message>
{
    private readonly Settings _settings;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IOptions<Settings> options, ILogger<MessageConsumer> logger)
    {
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task OnHandle(Message message)
    {
        _logger.LogConsumedMessage(message.Id);

        await Task.Delay(_settings.ConsumerDelay);
    }
}