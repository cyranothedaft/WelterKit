using System;
using WelterKit.Channels;



namespace WelterKit.Buses;

partial class FifoMessageBus<TMsg> {
   event Action<TMsg, bool>? IChannelEvents<TMsg>.OnWrite {
      add    => ( ( IChannelEvents<TMsg> )_busChannel ).OnWrite += value;
      remove => ( ( IChannelEvents<TMsg> )_busChannel ).OnWrite -= value;
   }
   
   event Action<TMsg>? IChannelEvents<TMsg>.OnRead {
      add    => ( ( IChannelEvents<TMsg> )_busChannel ).OnRead += value;
      remove => ( ( IChannelEvents<TMsg> )_busChannel ).OnRead -= value;
   }
}
