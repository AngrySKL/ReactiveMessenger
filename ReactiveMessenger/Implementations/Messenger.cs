using ReactiveMessenger.Interfaces;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveMessenger.Implementations;

public class Messenger : IMessenger
{
    #region Synchronous Messenger
    private readonly Dictionary<Type, object> _syncMap = [];
    public void RegisterSyncMessage<TSyncMessage>()
    {
        var type = typeof(TSyncMessage);
        if (_syncMap.ContainsKey(type))
        {
            throw new InvalidOperationException($"Synchronous message {type.Name} has already been registered!");
        }

        _syncMap.Add(type, new Subject<TSyncMessage>());
    }

    public void SendSyncMessage<TSyncMessage>(TSyncMessage payload)
    {
        var type = typeof(TSyncMessage);
        if (!_syncMap.TryGetValue(type, out object? value))
        {
            throw new InvalidOperationException($"Synchronous message {type.Name} has not been registered!");
        }

        (value as Subject<TSyncMessage>).OnNext(payload);
    }

    public IObservable<TSyncMessage> GetSyncMessenger<TSyncMessage>()
    {
        var type = typeof(TSyncMessage);
        if (!_syncMap.TryGetValue(type, out object? value))
        {
            throw new InvalidOperationException($"Synchronous message {type.Name} has not been registered!");
        }

        return (value as Subject<TSyncMessage>).ObserveOn(ThreadPoolScheduler.Instance).AsObservable();
    }
    #endregion

    #region Asynchronous Messenger
    private readonly Dictionary<Type, object> _asyncMap = [];
    public void RegisterAsyncMessage<TAsyncMessage>()
    {
        var type = typeof(TAsyncMessage);
        if (_asyncMap.ContainsKey(type))
        {
            throw new InvalidOperationException($"Asynchronous message {type.Name} has already been registered!");
        }

        _asyncMap.Add(type, new Subject<TAsyncMessage>());
    }

    public void SendAsyncMessage<TAsyncMessage>(TAsyncMessage payload)
    {
        var type = typeof(TAsyncMessage);
        if (!_asyncMap.TryGetValue(type, out object? value))
        {
            throw new InvalidOperationException($"Asynchronous message {type.Name} has not been registered!");
        }

        (value as Subject<TAsyncMessage>).OnNext(payload);
    }

    public IObservable<TAsyncMessage> GetAsyncMessenger<TAsyncMessage>()
    {
        var type = typeof(TAsyncMessage);
        if (!_asyncMap.TryGetValue(type, out object? value))
        {
            throw new InvalidOperationException($"Asynchronous message {type.Name} has not been registered!");
        }

        return (value as Subject<TAsyncMessage>).AsObservable();
    }
    #endregion
}
