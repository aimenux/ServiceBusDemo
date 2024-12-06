using Azure.Messaging.ServiceBus;
using Example03.Configuration;
using Example03.Consumers;
using Example03.Producers;
using Microsoft.Extensions.Options;

namespace Example03;

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
            return client;
        });

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<Settings>>().Value;
            var client = GetServiceBusClient(settings);
            var processor = GetServiceBusProcessor(client, settings);
            return processor;
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

    private static ServiceBusProcessor GetServiceBusProcessor(ServiceBusClient client, Settings settings)
    {
        var processorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = settings.MaxConcurrentCalls,
            AutoCompleteMessages = settings.AutoCompleteMessages
        };

        return client.CreateProcessor(settings.QueueName, processorOptions);
    }
}