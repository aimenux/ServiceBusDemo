using Azure.Messaging.ServiceBus;
using Example02.Configuration;
using Example02.Consumers;
using Example02.Producers;
using Microsoft.Extensions.Options;

namespace Example02;

public static class DependencyInjection
{
    public static void AddServices(this HostApplicationBuilder builder)
    {
        builder.AddSettings();
        builder.AddServiceBus();
    }

    private static void AddSettings(this HostApplicationBuilder builder)
    {
        builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SectionName));
        builder.Services.AddSingleton<IValidateOptions<Settings>, SettingsValidator>();
    }

    private static void AddServiceBus(this HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<MessageConsumer>();
        builder.Services.AddHostedService<MessageProducer>();

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>().Value;
            var client = GetServiceBusClient(settings);
            var receiver = GetServiceBusReceiver(client, settings);
            return receiver;
        });

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>().Value;
            var client = GetServiceBusClient(settings);
            var sender = GetServiceBusSender(client, settings);
            return sender;
        });
    }

    private static ServiceBusClient GetServiceBusClient(Settings settings)
    {
        var connectionString = settings.ConnectionString;

        var clientOptions = new ServiceBusClientOptions
        {
            TransportType = settings.TransportType
        };

        return new ServiceBusClient(connectionString, clientOptions);
    }

    private static ServiceBusReceiver GetServiceBusReceiver(ServiceBusClient client, Settings settings)
    {
        var receiverOptions = new ServiceBusReceiverOptions
        {
            ReceiveMode = settings.ReceiveMode
        };

        return client.CreateReceiver(settings.QueueName, receiverOptions);
    }

    private static ServiceBusSender GetServiceBusSender(ServiceBusClient client, Settings settings)
    {
        var senderOptions = new ServiceBusSenderOptions();

        return client.CreateSender(settings.QueueName, senderOptions);
    }
}