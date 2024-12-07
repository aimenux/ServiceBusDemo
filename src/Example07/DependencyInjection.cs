using Example07.Configuration;
using Example07.Contracts;
using Example07.Extensions;
using Example07.Producers;
using Microsoft.Extensions.Options;
using SlimMessageBus.Host;
using SlimMessageBus.Host.AzureServiceBus;
using SlimMessageBus.Host.Serialization.SystemTextJson;

namespace Example07;

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
        var settings = builder.Configuration.GetSettings();

        builder.Services.AddSlimMessageBus(cfg =>
        {
            cfg.WithProviderServiceBus(x =>
            {
                x.ConnectionString = settings.ConnectionString;
            });

            cfg.AddConsumers();
            cfg.AddJsonSerializer();
            cfg.Consume<Message>(x => x.Queue(settings.QueueName));
            cfg.Produce<Message>(x => x.DefaultQueue(settings.QueueName));
        });

        builder.Services.AddHostedService<MessageProducer>();
    }

    private static Settings GetSettings(this ConfigurationManager configuration)
    {
        var settings = new Settings();
        configuration.GetSection(Settings.SectionName).Bind(settings);
        return settings;
    }
}