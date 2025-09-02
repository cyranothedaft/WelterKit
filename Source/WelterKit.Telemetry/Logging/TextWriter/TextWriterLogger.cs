using System;
using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.Logging;


namespace WelterKit.Telemetry.Logging.TextWriter;

public class TextWriterLogger : ILogger {

   internal delegate void LogActionDelegate(LogLevel logLevel, EventId eventId, string message);

   private readonly string _categoryName;
   private readonly IExternalScopeProvider _scopeProvider;
   private readonly Func<LogLevel, bool> _isEnabledFunc;
   private readonly LogActionDelegate _logAction;


   private static readonly ImmutableArray<string> _logLevelAbbreviations =
      [ // these match the built-in console log provider
         "trce", // Trace
         "dbug", // Debug
         "info", // Information
         "warn", // Warning
         "fail", // Error
         "crit", // Critical
         "none", // None
      ];
      //[
      //   "tra", // Trace
      //   "dbg", // Debug
      //   "inf", // Information
      //   "wrn", // Warning
      //   "err", // Error
      //   "crt", // Critical
      //   "non", // None
      //];


   public TextWriterLogger(string categoryName, IExternalScopeProvider scopeProvider, Func<LogLevel, bool> isEnabledFunc, System.IO.TextWriter writer, bool forceSingleLine) {
      _categoryName  = categoryName;
      _scopeProvider = scopeProvider;
      _isEnabledFunc = isEnabledFunc;

      Func<string, string> replaceNewlinesFunc = forceSingleLine
                                                       ? replaceNewlines
                                                       : msg => msg;

      _logAction = (logLevel, eventId, message) => writeLine(scopeProvider, writer, replaceNewlinesFunc, logLevel, categoryName, eventId, message);
   }


   public TextWriterLogger(string categoryName, System.IO.TextWriter writer, Func<LogLevel, bool> isEnabledFunc, bool forceSingleLine)
         : this(categoryName, new LoggerExternalScopeProvider(), isEnabledFunc, writer, forceSingleLine) { }


   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
      if (IsEnabled(logLevel))
         _logAction(logLevel, eventId, formatter(state, exception));
   }


   public bool IsEnabled(LogLevel logLevel) 
      => _isEnabledFunc(logLevel);


   public IDisposable BeginScope<TState>(TState state) where TState : notnull {
      return _scopeProvider.Push(state);
   }


   private static void writeLine(IExternalScopeProvider scopeProvider, System.IO.TextWriter writer, Func<string, string> replaceNewlinesFunc,
                                 LogLevel logLevel, string category, EventId eventId, string msg) {
      writer.WriteLine(formatThis(logLevel,
                                  category,
                                  eventId,
                                  formatScope(scopeProvider) + replaceNewlinesFunc(msg)
                                  ));
      writer.Flush();
   }


   // TODO: allow overriding this with setting/option
   private static string formatThis(LogLevel logLevel, string category, EventId eventId, string msg)
      => string.Format("{0:yyyy-MM-dd HH:mm:ss.fffff} {1}: {2}[{3}] {4}",
                       DateTimeOffset.Now,
                       abbreviate(logLevel),
                       category,
                       eventId,
                       msg);


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
