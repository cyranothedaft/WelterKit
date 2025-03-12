using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;



namespace WelterKit.Channels;

partial class FifoChannel<T> {
   /// <summary>
   /// TODO: explain why these are needed/wanted
   /// </summary>
   private class ChannelWriterWrapper : IChannelWriter<T> {
      private readonly ChannelWriter<T> _channelWriter;
      public ChannelWriterWrapper(ChannelWriter<T> channelWriter) { _channelWriter = channelWriter; }
      public bool Write(T submission) => _channelWriter.TryWrite(submission);
      public bool Complete()          => _channelWriter.TryComplete();
   }

   private class ChannelReaderWrapper : IChannelReader<T> {
      private readonly ChannelReader<T> _channelReader;
      public ChannelReaderWrapper(ChannelReader<T> channelReader) { _channelReader = channelReader; }
      public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default) => _channelReader.ReadAllAsync(cancellationToken);
      public int Count => _channelReader.Count;
   }

}
