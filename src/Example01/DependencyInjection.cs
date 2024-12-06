using Azure.Messaging.ServiceBus;
using Example01.Configuration;
using Example01.Consumers;
using Example01.Producers;
using Microsoft.Extensions.Options;

namespace Example01;

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
}