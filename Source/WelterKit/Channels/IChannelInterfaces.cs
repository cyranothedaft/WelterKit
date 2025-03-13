using System;
using System.Collections.Generic;
using System.Threading;



namespace WelterKit.Channels;

public interface IChannel<T> : IChannelEvents<T> {
   IChannelWriter<T> Writer { get; }
   IChannelReader<T> Reader { get; }
}


public interface IChannelReader<out T> {
   IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default);
   int Count { get; }
}


public interface IChannelWriter<in T> {
   bool Write(T submission);
   bool Complete();
}


public interface IChannelEvents<out T> {
   event Action<T, bool>? OnWrite;
   event Action<T>? OnRead;
}
