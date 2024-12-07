using Example06.Configuration;
using Example06.Consumers;
using Example06.Contracts;
using Example06.Producers;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.Routing.TypeBased;

namespace Example06;

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

        builder.Services.AddRebus(cfg =>
        {
            cfg.Transport(x => x.UseAzureServiceBus(settings.ConnectionString, settings.QueueName));
            cfg.Routing(r => r.TypeBased().Map<Message>(settings.QueueName));
            return cfg;
        });

        builder.Services.AddRebusHandler<MessageConsumer>();

        builder.Services.AddHostedService<MessageProducer>();
    }

    private static Settings GetSettings(this ConfigurationManager configuration)
    {
        var settings = new Settings();
        configuration.GetSection(Settings.SectionName).Bind(settings);
        return settings;
    }
}