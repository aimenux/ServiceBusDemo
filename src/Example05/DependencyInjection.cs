using Example05.Configuration;
using Example05.Consumers;
using Example05.Contracts;
using Example05.Producers;
using Microsoft.Extensions.Options;
using Wolverine;
using Wolverine.AzureServiceBus;

namespace Example05;

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

        builder.UseWolverine(options =>
        {
            options.UseAzureServiceBus(settings.ConnectionString).AutoProvision();
            options.PublishMessage<Message>().ToAzureServiceBusQueue(settings.QueueName);
            options.ListenToAzureServiceBusQueue(settings.QueueName);
        });

        builder.Services.AddSingleton<MessageConsumer>();

        builder.Services.AddHostedService<MessageProducer>();
    }

    private static Settings GetSettings(this ConfigurationManager configuration)
    {
        var settings = new Settings();
        configuration.GetSection(Settings.SectionName).Bind(settings);
        return settings;
    }
}