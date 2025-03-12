using System;
using System.Collections.Generic;
using System.Threading;


namespace WelterKit.Buses;

public partial class FifoMessageBus<TMsg> : IBusReader<TMsg> {

   IAsyncEnumerable<TMsg> IBusReader<TMsg>.ReadToCompletionAsync(CancellationToken cancellationToken)
      => _busChannel.ReadAllAsync(cancellationToken);


   int IBusReader<TMsg>.GetCount()
      => _busChannel.Count();

}
