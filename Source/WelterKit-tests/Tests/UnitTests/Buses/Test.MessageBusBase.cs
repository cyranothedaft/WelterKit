using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Buses;


namespace WelterKit_Tests.Tests_Unit.Buses;

[TestClass()]
public abstract class Test_MessageBusBase {
   private readonly ILogger? _logger;

   protected abstract IMessageBus<object> CreateTestBus();


   protected Test_MessageBusBase(ILogger? logger = null) {
      _logger = logger;
   }


   [TestMethod]
   public Task CountsOneMessageAfterWritingSingleMessage()
      => testBusFinalStateAfterActionsAsync(CreateTestBus(),
                                            doTestActions: (_, busWriter)
                                                                 => {
                                                              busWriter.Write("test message");
                                                              busWriter.Stop();
                                                           },
                                            finalStats_expected: new MessageBusStats(TotalMessageCount: 1,
                                                                                     UnroutableMessageCount: 1,
                                                                                     LeftoverMessageCount: 0), // even undeliverable letters are dequeued
                                            logger: _logger
                                           );


   [TestMethod]
   public Task CountsOneMessageAfterWritingSingleMessageWithSubscriber()
      => testBusFinalStateAfterActionsAsync(CreateTestBus(),
                                            doTestActions: (publisher, busWriter)
                                                                 => {
                                                              publisher.SubscribeTypedMessageHandler<string>(msg => _logger?.LogTrace("Got message: {msg}", msg));
                                                              busWriter.Write("test message");
                                                              busWriter.Stop();
                                                           },
                                            finalStats_expected: new MessageBusStats(TotalMessageCount: 1,
                                                                                     UnroutableMessageCount: 0,
                                                                                     LeftoverMessageCount: 0), // even undeliverable letters are dequeued
                                            logger: _logger
                                           );


   [TestMethod]
   public Task CountsOneMessageAfterWritingSingleMessageWithEarlySubscriber()
      => testBusFinalStateAfterActionsAsync(CreateTestBus(),
                                            doBeforeTestActions: (publisher, _) => {
                                                              publisher.SubscribeTypedMessageHandler<string>(msg => _logger?.LogTrace("Got message: {msg}", msg));
                                                                 },
                                            doTestActions: (_, busWriter)
                                                                 => {
                                                              busWriter.Write("test message");
                                                              busWriter.Stop();
                                                           },
                                            finalStats_expected: new MessageBusStats(TotalMessageCount: 1,
                                                                                     UnroutableMessageCount: 0,
                                                                                     LeftoverMessageCount: 0), // even undeliverable letters are dequeued
                                            logger: _logger
                                           );


   [TestMethod]
   [ExpectedException(typeof( Exception ))]
   public Task MessageRoutedWithEarlySubscriber()
      => testBusFinalStateAfterActionsAsync(CreateTestBus(),
                                            doBeforeTestActions: (publisher, _) => {
                                                                    publisher.SubscribeTypedMessageHandler<string>(msg => throw new Exception($"Message successfully routed: [{msg}]"));
                                                                 },
                                            doTestActions: (_, busWriter)
                                                                 => {
                                                              busWriter.Write("test message");
                                                              busWriter.Stop();
                                                           },
                                            finalStats_expected: new MessageBusStats(TotalMessageCount: 1,
                                                                                     UnroutableMessageCount: 0,
                                                                                     LeftoverMessageCount: 0), // even undeliverable letters are dequeued
                                            logger: _logger
                                           );


   [TestMethod]
   public Task CountsZeroMessageAfterCompletingUnusedBus()
      => testBusFinalStateAfterActionsAsync(CreateTestBus(),
                                            doTestActions: (_, busWriter)
                                                                 => {
                                                              busWriter.Stop();
                                                           },
                                            finalStats_expected: new MessageBusStats(TotalMessageCount: 0,
                                                                                     UnroutableMessageCount: 0,
                                                                                     LeftoverMessageCount: 0),
                                            logger: _logger
                                           );


   private static async Task testBusFinalStateAfterActionsAsync<TMsg>(IMessageBus<TMsg> bus, MessageBusStats finalStats_expected,
                                                                      Action<IPublisher<TMsg>, IBusWriter<TMsg>>? doBeforeTestActions = null,
                                                                      Action<IPublisher<TMsg>, IBusWriter<TMsg>>? doTestActions = null,
                                                                      ILogger? logger = null){

      doBeforeTestActions?.Invoke(bus.Publisher, bus.Writer);
      Task<MessageBusStats> busTask = bus.StartAndAwaitCompletionAsync();

      doTestActions?.Invoke(bus.Publisher, bus.Writer);
      MessageBusStats finalStats_actual = await busTask;

      Assert.AreEqual(finalStats_expected, finalStats_actual);
   }
}
