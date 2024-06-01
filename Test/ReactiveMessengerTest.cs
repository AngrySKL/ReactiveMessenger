using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReactiveMessenger;
using ReactiveMessenger.Interfaces;
using Test.Messages;

namespace Test;

public class ReactiveMessengerTest
{
    [Fact]
    public void ShouldRegister()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddReactiveMessenger();

        var applicationBuilder = new ApplicationBuilder(serviceCollection.BuildServiceProvider());
        applicationBuilder.UseReactiveMessenger();
        applicationBuilder.Build();

        var messenger = applicationBuilder.ApplicationServices.GetRequiredService<IMessenger>();
        var asyncAct = () => messenger.GetAsyncMessenger<FakeAsyncMessage>();
        asyncAct.Should().NotThrow<InvalidOperationException>();

        var syncAct = () => messenger.GetSyncMessenger<FakeSyncMessage>();
        syncAct.Should().NotThrow<InvalidOperationException>();
    }
}