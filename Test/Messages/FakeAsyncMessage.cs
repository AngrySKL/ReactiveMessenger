using ReactiveMessenger.Attributes;

namespace Test.Messages;

[AsyncMessage]
public class FakeAsyncMessage
{
    public int Vaue { get; set; }
}
