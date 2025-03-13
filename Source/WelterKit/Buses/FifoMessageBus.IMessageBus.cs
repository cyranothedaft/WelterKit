using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using WelterKit.Std.Functional;



namespace WelterKit.Buses;


partial class FifoMessageBus<TMsg> : IMessageBus<TMsg> {


   public IPublisher<TMsg> Publisher => this;
   public IBusWriter<TMsg> Writer => this;
   public IBusReader<TMsg> Reader => this;


   public Task<MessageBusStats> StartAndAwaitCompletionAsync(bool throwOnUnroutableMessage = false, CancellationToken cancellationToken = default)
      => startAndAwaitCompletionAsync(this, cancellationToken, throwOnUnroutableMessage, _logger);


   private static async Task<MessageBusStats> startAndAwaitCompletionAsync(FifoMessageBus<TMsg> bus,
                                                                           CancellationToken cancellationToken,
                                                                           bool throwOnUnroutableMessage,
                                                                           ILogger? logger) {
      int messageCount = 0,
          unroutableCount = 0;

      logger?.LogTrace("Bus loop starting");
      //TODO===logger?.LogTrace(ClassName, $"Bus loop starting");

      await foreach ( TMsg msg in bus.Reader.ReadToCompletionAsync(cancellationToken) ) {
         logger?.LogDebug("--> Read message: {msg}", msg);

         messageCount++;
         // if ( msg is string s && s == "test message" )
         //    Console.WriteLine("this one");
         (bool wasRouted, bool shouldBeRouted) = tryRouteMsg(bus.Publisher.GetSubscriberListReader, msg, logger);
         if ( shouldBeRouted && !wasRouted ) {
            ++unroutableCount;
            logger?.LogWarning("Channel item [{item}] could not be routed", msg);
            if ( throwOnUnroutableMessage )
               throw new InvalidOperationException($"Channel item [{msg}] could not be routed");

            //TODO===logger?.LogWarning(ClassName, $"Channel item [{item}] could not be routed");
         }
      }

      logger?.LogTrace($"Bus loop ended");
      //TODO===logger?.LogTrace(ClassName, $"Bus loop ended");

      var stats = new MessageBusStats(TotalMessageCount: messageCount,
                                      UnroutableMessageCount: unroutableCount,
                                      LeftoverMessageCount: bus.Reader.GetCount());
      logger?.LogInformation("Bus stats: {stats}", stats);
      //TODO===logger?.LogInfo(ClassName, "Bus stats: {stats}", msgArgs: stats);

      return stats;
   }


   private static (bool wasRouted, bool shouldBeRouted) tryRouteMsg(Func<ISubscriberListReader<TMsg>> getSubscriberListFunc, TMsg msg, ILogger? logger) {
      var subscriberListReader = getSubscriberListFunc();
      if ( subscriberListReader.Count == 0 ) {
         logger?.LogWarning("Attempted to route message [{msg}], but subscriber list is empty.", msg);
         return ( false, true );
      }
      else {
         return tryGetHandlerForMsg(subscriberListReader, msg, logger)
               .Map(foundAction => {
                       foundAction();
                       return ( true, true );
                    })
               .Reduce(( false, true ));
      }
   }


   private static Maybe<Action> tryGetHandlerForMsg(ISubscriberListReader<TMsg> subscriberListReader, TMsg msg, ILogger? log) {
      var subscriber = subscriberListReader.TryGet(msg);
      log?.LogTrace("got subscriber for msg (from list count={listCount})? {subscriber}", subscriberListReader.Count, subscriber);

      return subscriber
            .Map<ISubscriber<TMsg>, HandleGeneralMessageDelegate<TMsg>>(sub => sub.HandleMessage)
            .Map<HandleGeneralMessageDelegate<TMsg>, Action>(handleMsg => () => handleMsg(msg));
   }
}
