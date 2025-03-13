using System.Collections.Generic;
using System.Threading;


namespace WelterKit.Buses;

public interface IBusReader<out TMsg> {
   IAsyncEnumerable<TMsg> ReadToCompletionAsync(CancellationToken cancellationToken = default);
   int GetCount();
}
