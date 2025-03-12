using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;



namespace WelterKit.Buses;

public partial class FifoMessageBus<TMsg> : IPublisher<TMsg> {
   public void SubscribeTypedMessageHandler<TSubMsg>(HandleSpecificMessageDelegate<TSubMsg> handleMessage, [CallerMemberName] string? callerMemberName = null)
         where TSubMsg : TMsg {
      _logger?.LogTrace(nameof( SubscribeTypedMessageHandler ) + "<{tSubMsg}> callerMemberName:{callerMemberName}", typeof( TSubMsg ).Name, callerMemberName);
      _subscribers = subscribe(_subscribers, handleMessage);

      static SubscriberList<TMsg> subscribe(SubscriberList<TMsg> subscribers, HandleSpecificMessageDelegate<TSubMsg> handleSpecificMessage) {
         var newSubscriberList = subscribers.TryAdd(handleSpecificMessage, out bool success);
         if ( !success )
            throw new Exception($"Attempt to add type [{typeof( TSubMsg ).Name}] to subscriber list was unsuccessful.");
         return newSubscriberList;
      }
   }


   public ISubscriberListReader<TMsg> GetSubscriberListReader() => _subscribers;
}
