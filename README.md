# ReactiveMessenger
 
A lightweight in-process messenger.

# Usage

## Create your own message and denote it with AsyncMessage for async messages or SyncMessage for sync messages attributes
`[AsyncMessage]`  
`public class FakeAsyncMessage { }`  

`[SyncMessage]`  
`public class FakeSyncMessage { }`

## Add ReactiveMessenger into DI container
`services.AddReactiveMessenger()`

## UseReactiveMessenger will load all denoted message into reactive messenger
`app.UseReactiveMesenger()`

### Or you can register by your own
`messenger.RegisterSyncMessage<FakeSyncMessage>()`

## Inject `IMessenger` anywhere you want and then

### Send
`messenger.SendSyncMessage<FakeSyncMessage>(new())`

### Subscribe
`messenger.GetSyncMessenger<FakeSyncMessage>(new()).Subscribe(payload => { //payload is FakeSyncMessage })`

# What's the difference between synchronous and asychronous messenger?
Synchronous messenger sends messages in sequence on the same thread the `SendSyncMessage` is called while asychronous messenger sends messages in sequence on a different thread than the one called `SendAsyncMessage`.
