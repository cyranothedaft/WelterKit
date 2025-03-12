using Microsoft.Extensions.Logging;



namespace WelterKit.Buses;

public partial class FifoMessageBus<TMsg> : IBusWriter<TMsg> {
   void IBusWriter<TMsg>.Write(TMsg message) {
      _logger?.LogDebug("<-- Write message: {message}", message);
      _busChannel.Write(message);
   }


   void IBusWriter<TMsg>.Stop() {
      _busChannel.Complete();
   }
}
