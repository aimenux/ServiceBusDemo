using System.Reflection;
using SlimMessageBus;
using SlimMessageBus.Host;

namespace Example07.Extensions;

public static class SlimMessageBusExtensions
{
    private static Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

    public static void AddConsumers(this MessageBusBuilder mbb)
    {
        mbb.AddServicesFromAssembly(CurrentAssembly, IsConsumerType);
    }

    private static bool IsConsumerType(this Type type)
    {
        return type
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));
    }
}