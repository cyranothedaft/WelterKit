using System;
using System.Threading;
using System.Threading.Tasks;
using WelterKit.Channels;



namespace WelterKit.Buses;

public interface IMessageBus<TMsg> : IChannelEvents<TMsg> {

   IPublisher<TMsg> Publisher { get; }
   IBusWriter<TMsg> Writer { get; }
   IBusReader<TMsg> Reader { get; }

   Task<MessageBusStats> StartAndAwaitCompletionAsync(bool throwOnUnroutableMessage = false, CancellationToken cancellationToken = default);
}
