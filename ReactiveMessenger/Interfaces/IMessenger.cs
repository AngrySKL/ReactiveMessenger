namespace ReactiveMessenger.Interfaces;

public interface IMessenger
{
    #region Synchoronous Messsenger
    void RegisterSyncMessage<TSyncMessage>();

    void SendSyncMessage<TSyncMessage>(TSyncMessage payload);

    IObservable<TSyncMessage> GetSyncMessenger<TSyncMessage>();
    #endregion

    #region Asynchronous Messenger
    void RegisterAsyncMessage<TAsyncMessage>();

    void SendAsyncMessage<TAsyncMessage>(TAsyncMessage payload);

    IObservable<TAsyncMessage> GetAsyncMessenger<TAsyncMessage>();
    #endregion
}
