﻿using ReactiveMessenger.Attributes;

namespace Test.Messages;

[SyncMessage]
public class FakeSyncMessage
{
    public int Vaue { get; set; }
}
