using Microsoft.Extensions.Options;

namespace Example01.Configuration;

public sealed class SettingsValidator : IValidateOptions<Settings>
{
    public ValidateOptionsResult Validate(string? name, Settings? settings)
    {
        if (settings is null)
        {
            return ValidateOptionsResult.Fail($"{nameof(Settings)} is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.QueueName))
        {
            return ValidateOptionsResult.Fail($"{nameof(Settings.QueueName)} is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            return ValidateOptionsResult.Fail($"{nameof(Settings.ConnectionString)} is required.");
        }

        return ValidateOptionsResult.Success;
    }
}