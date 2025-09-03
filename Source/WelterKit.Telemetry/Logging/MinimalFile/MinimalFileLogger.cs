using System;
using System.IO;
using Microsoft.Extensions.Logging;
using WelterKit.Telemetry.Logging.TextWriter;


namespace WelterKit.Telemetry.Logging.MinimalFile;

internal class MinimalFileLogger : TextWriterLogger {
   internal MinimalFileLogger(string categoryName, IExternalScopeProvider scopeProvider, Func<LogLevel, bool> isEnabledFunc, StreamWriter streamWriter, bool forceSingleLine)
         : base(categoryName, scopeProvider, isEnabledFunc, streamWriter, forceSingleLine) { }
}
