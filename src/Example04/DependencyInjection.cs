using System.Reflection;
using Example04.Configuration;
using Example04.Producers;
using MassTransit;
using MassTransit.AzureServiceBusTransport.Configuration;
using Microsoft.Extensions.Options;

namespace Example04;

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
        var hostSettings = builder.Configuration.GetHostSettings();

        builder.Services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();
            configure.AddConsumers(Assembly.GetEntryAssembly());
            configure.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(hostSettings);
                cfg.ConfigureEndpoints(context);
            });
        });

        builder.Services.AddHostedService<MessageProducer>();
    }

    private static HostSettings GetHostSettings(this ConfigurationManager configuration)
    {
        var settings = new Settings();
        configuration.GetSection(Settings.SectionName).Bind(settings);
        var hostSettings = new HostSettings
        {
            ConnectionString = settings.ConnectionString,
            TransportType = settings.TransportType
        };
        return hostSettings;
    }
}