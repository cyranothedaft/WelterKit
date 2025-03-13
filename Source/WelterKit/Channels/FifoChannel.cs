using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;


namespace WelterKit.Channels;


public partial class FifoChannel<T> : IChannel<T> {
   private readonly ILogger? _logger;
   private readonly Channel<T> _channel; // TODO: ensure (somehow) that this is FIFO!

   public event Action<T, bool>? OnWrite;
   public event Action<T>? OnRead;

   public IChannelWriter<T> Writer { get; }
   public IChannelReader<T> Reader { get; }


   public FifoChannel(ILogger? logger = null)
         : this(logger, buildChannel()) { }


   private FifoChannel(ILogger? logger, Channel<T> channel) {
      _logger  = logger;
      _channel = channel;
      Writer = new ChannelWriterWrapper(channel.Writer);
      Reader = new ChannelReaderWrapper(channel.Reader);
   }


   public void Complete() {
      bool success = Writer.Complete();

      _logger?.LogTrace("Channel completed (success:{success})", success);
   }


   public int Count()
      => Reader.Count;


   public async IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default) {
      await foreach ( T item in Reader.ReadAllAsync(cancellationToken) ) {
         _logger?.LogTrace("Read [{item}]", item);
         OnRead?.Invoke(item);
         yield return item;
      }
   }


   public void Write(T item) {
      bool success = Writer.Write(item);
      _logger?.LogTrace("Write [{item}], success:{success}", item, success);
      OnWrite?.Invoke(item, success);
      // TODO: use bool result
   }


   public void DisposalFlush(ILogger? logger = null)
      => disposalFlush(_channel.Reader, logger);


   private static Channel<T> buildChannel()
      => Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
                                       {
                                          SingleWriter = true,
                                          SingleReader = false,
                                       });


   private static void disposalFlush(ChannelReader<T> channelReader, ILogger? logger = null) {
      int numRead = 0,
          initialNumRemaining = channelReader.Count,
          numRemaining = initialNumRemaining;

      logger?.LogTrace("DisposalFlush - emergency disposal channel flush ({numRemaining} items)", numRemaining);

      while ( numRemaining > 0 && channelReader.TryRead(out T? item) ) {
         logger?.LogTrace($"DisposalFlush - read: [{++numRead:00}] {item?.ToString() ?? "<null>"}");
         --numRemaining;
      }

      logger?.LogTrace("DisposalFlush - emergency disposal queue flushed {numRead} items; left {numRemaining} of {initialNumRemaining} items behind",
                       numRead, numRemaining, initialNumRemaining);
      if ( initialNumRemaining > 0 )
         logger?.LogDebug("On disposal, {initialNumRemaining} elements were left in the queue; {numRead} were flushed, and {numRemaining} were left behind",
                          initialNumRemaining, numRead, numRemaining);
   }
}
