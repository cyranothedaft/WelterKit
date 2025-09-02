using System;
using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.Logging;


namespace WelterKit.Telemetry.Logging.TextWriter;

public class TextWriterLogger : ILogger {
   private readonly IExternalScopeProvider _scopeProvider;
   private readonly Func<LogLevel, bool> _isEnabledFunc;
   private readonly Action<LogLevel, string> _logAction;


   private static readonly ImmutableArray<string> _logLevelAbbreviations =
      [
         "tra", // Trace
         "dbg", // Debug
         "inf", // Information
         "wrn", // Warning
         "err", // Error
         "crt", // Critical
         "non", // None
      ];


   public TextWriterLogger(IExternalScopeProvider scopeProvider, Func<LogLevel, bool> isEnabledFunc, TextWriter writer, bool forceSingleLine) {
      _scopeProvider = scopeProvider;
      _isEnabledFunc = isEnabledFunc;

      Func<string, string> replaceNewlinesFunc = forceSingleLine
                                                       ? replaceNewlines
                                                       : msg => msg;

      _logAction = (logLevel, message) => writeLine(scopeProvider, writer, replaceNewlinesFunc, logLevel, message);
   }


   public TextWriterLogger(TextWriter writer, Func<LogLevel, bool> isEnabledFunc, bool forceSingleLine)
         : this(new LoggerExternalScopeProvider(), isEnabledFunc, writer, forceSingleLine) { }


   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
      if (IsEnabled(logLevel))
         _logAction(logLevel, formatter(state, exception));
   }


   public bool IsEnabled(LogLevel logLevel) 
      => _isEnabledFunc(logLevel);


   public IDisposable BeginScope<TState>(TState state) where TState : notnull {
      return _scopeProvider.Push(state);
   }


   private static void writeLine(IExternalScopeProvider scopeProvider, TextWriter writer, Func<string, string> replaceNewlinesFunc, LogLevel logLevel, string msg) {
      writer.WriteLine(formatThis(logLevel, formatScope(scopeProvider) + replaceNewlinesFunc(msg)));
      writer.Flush();
   }


   // TODO: allow overriding this with setting/option
   private static string formatThis(LogLevel logLevel, string msg)
      => string.Format("[{0:yyyy-MM-dd HH:mm:ss.fffff}] {1}: {2}", DateTimeOffset.Now, abbreviate(logLevel), msg);


   private static string formatScope(IExternalScopeProvider scopeProvider) {
      StringBuilder sb = new StringBuilder();
      scopeProvider.ForEachScope((scopeObject, stringbuilder) => stringbuilder.AppendFormat("=> {0} ", scopeObject?.ToString()), sb);
      return sb.ToString();
   }


   private static string replaceNewlines(string str)
      => str.Replace('\r', ' ')
            .Replace('\n', ' ');


   private static string abbreviate(LogLevel logLevel)
      => _logLevelAbbreviations[(int)logLevel];
}
