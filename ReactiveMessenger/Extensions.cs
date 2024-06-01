using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReactiveMessenger.Attributes;
using ReactiveMessenger.Implementations;
using ReactiveMessenger.Interfaces;
using System.Reflection;

namespace ReactiveMessenger;

public static class Extensions
{
    public static IServiceCollection AddReactiveMessenger(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger, Messenger>();

        return services;
    }

    public static IApplicationBuilder UseReactiveMessenger(this IApplicationBuilder app)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var allTypes = new List<Type>();
        foreach (var assembly in assemblies)
        {
            allTypes.AddRange(assembly.GetTypes());
        }

        var asyncMessages = new List<Type>();
        var syncMessages = new List<Type>();

        foreach (var type in allTypes)
        {
            if (type.GetCustomAttributes<AsyncMessageAttribute>().Any())
            {
                asyncMessages.Add(type);
                continue;
            }

            if (type.GetCustomAttributes<SyncMessageAttribute>().Any())
            {
                syncMessages.Add(type);
                continue;
            }
        }

        var messenger = app.ApplicationServices.GetRequiredService<IMessenger>();
        Register(asyncMessages, "RegisterAsyncMessage", messenger);
        Register(syncMessages, "RegisterSyncMessage", messenger);

        return app;
    }

    private static void Register(List<Type> messageTypes, string methodName, IMessenger instance)
    {
        var messengerType = typeof(Messenger);
        foreach (var messageType in messageTypes)
        {
            var register = messengerType.GetMethod(methodName);
            var constructedMethod = register.MakeGenericMethod(messageType);
            constructedMethod.Invoke(instance, []);
        }
    }
}
