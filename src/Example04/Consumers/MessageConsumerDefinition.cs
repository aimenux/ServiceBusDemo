using Example04.Configuration;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Example04.Consumers;

public sealed class MessageConsumerDefinition : ConsumerDefinition<MessageConsumer>
{
    private readonly Settings _settings;

    public MessageConsumerDefinition(IOptions<Settings> options)
    {
        _settings = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        EndpointName = options.Value.QueueName;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MessageConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Interval(_settings.RetrySettings.RetryCount, _settings.RetrySettings.RetryDelay));
    }
}