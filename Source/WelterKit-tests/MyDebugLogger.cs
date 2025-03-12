using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;



namespace WelterKit_Tests;

internal class MyDebugLogger : ILogger {
   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
      Debug.WriteLine(string.Concat(logLevel.Prefix(),
                                    formatter(state, exception)));
   }


   public bool IsEnabled(LogLevel logLevel) => true;


   public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
}



internal static class LogLevelExtensions {
   internal static string Prefix(this LogLevel logLevel)
      => logLevel switch
         {
            LogLevel.Trace       => "#   ",
            LogLevel.Debug       => "*   ",
            LogLevel.Information => "-   ",
            LogLevel.Warning     => "[!] ",
            LogLevel.Error       => "<!> ",
            LogLevel.Critical    => "!!! ",
            LogLevel.None        => string.Empty,
            _                    => throw new ArgumentOutOfRangeException(nameof( logLevel ), logLevel, null)
         };
}
