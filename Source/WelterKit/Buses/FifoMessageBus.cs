using System;
using WelterKit.Channels;
using Microsoft.Extensions.Logging;
using WelterKit.StaticUtilities;


namespace WelterKit.Buses;

public partial class FifoMessageBus<TMsg> : IDisposable {

   private readonly ILogger? _logger;
   private readonly FifoChannel<TMsg> _busChannel;
   private SubscriberList<TMsg> _subscribers;


   public FifoMessageBus(ILogger? logger = null)
         : this(logger,
                new FifoChannel<TMsg>(logger?.WithPrefix("[chn] "))) { }


   private FifoMessageBus(ILogger? logger, FifoChannel<TMsg> busChannel) {
      _logger      = logger;
      _busChannel  = busChannel;
      _subscribers = new SubscriberList<TMsg>(logger?.WithPrefix($"[subs<{typeof( TMsg ).Name}>] "));
   }


   public void Dispose() {
      // in case it isn't empty, attempt to flush the channel and log the flushed items
      _busChannel.DisposalFlush(_logger);
   }
}
