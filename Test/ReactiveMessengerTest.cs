using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReactiveMessenger;
using ReactiveMessenger.Implementations;
using ReactiveMessenger.Interfaces;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

    [Fact]
    public async void ShouldSendAsyncMessage()
    {
        var messenger = new Messenger();
        messenger.RegisterAsyncMessage<FakeAsyncMessage>();

        var stopSubject = new Subject<Unit>();
        FakeAsyncMessage? message = default;
        messenger
            .GetAsyncMessenger<FakeAsyncMessage>()
            .Subscribe(paylaod =>
            {
                message = paylaod;
                stopSubject.OnNext(Unit.Default);
            });

        await Observable
            .Interval(TimeSpan.FromSeconds(1))
            .TakeUntil(stopSubject)
            .ForEachAsync(_ =>
            {
                messenger.SendAsyncMessage(new FakeAsyncMessage() { Vaue = 66 });
            });

        message.Should().NotBeNull();
        message.Vaue.Should().Be(66);
    }

    [Fact]
    public async void ShoulldSendSyncMessage()
    {
        var messenger = new Messenger();
        messenger.RegisterSyncMessage<FakeSyncMessage>();

        var stopSubject = new Subject<Unit>();
        FakeSyncMessage? message = default;
        messenger
            .GetSyncMessenger<FakeSyncMessage>()
            .Subscribe(paylaod =>
            {
                message = paylaod;
                stopSubject.OnNext(Unit.Default);
            });

        await Observable
            .Interval(TimeSpan.FromSeconds(1))
            .TakeUntil(stopSubject)
            .ForEachAsync(_ =>
            {
                messenger.SendSyncMessage(new FakeSyncMessage() { Vaue = 66 });
            });

        message.Should().NotBeNull();
        message.Vaue.Should().Be(66);
    }
}