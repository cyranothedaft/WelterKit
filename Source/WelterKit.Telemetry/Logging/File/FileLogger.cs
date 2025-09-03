using System;
using Microsoft.Extensions.Logging;
using WelterKit.Telemetry.Logging.TextWriter;


namespace WelterKit.Telemetry.Logging.File;

internal class FileLogger : TextWriterLogger {
   internal FileLogger(string categoryName, IExternalScopeProvider scopeProvider, Func<LogLevel, bool> isEnabledFunc, StreamWriter streamWriter, bool forceSingleLine)
         : base(categoryName, scopeProvider, isEnabledFunc, streamWriter, forceSingleLine) { }
}
