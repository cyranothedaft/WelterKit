using System;
using Microsoft.Extensions.Logging;



namespace WelterKit.StaticUtilities;

public static class LoggerExtensions {
   public static ILogger WithPrefix(this ILogger logger, string prefix)
      => new NestedLogger(logger, msg => prefix + msg);



   private class NestedLogger : ILogger {
      private readonly ILogger _inner;
      private readonly Func<string, string> _formatter;

      public NestedLogger(ILogger inner, Func<string, string> formatter) {
         _inner     = inner;
         _formatter = formatter;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
         => _inner.Log(logLevel, eventId, state, exception, (s, e) => _formatter(formatter(s, e)));

      public bool IsEnabled(LogLevel logLevel) => _inner.IsEnabled(logLevel);
      public IDisposable BeginScope<TState>(TState? state) => _inner.BeginScope(state);
   }
}
