using System;
using System.IO;
using Microsoft.Extensions.Logging;
using WelterKit.Telemetry.Logging.TextWriter;


namespace WelterKit.Telemetry.Logging.File;

internal class FileLogger : TextWriterLogger {
   internal FileLogger(IExternalScopeProvider scopeProvider, Func<LogLevel, bool> isEnabledFunc, StreamWriter streamWriter,
                       bool forceSingleLine // TODO: move to settings/options class
   ) : base(scopeProvider, isEnabledFunc, streamWriter, forceSingleLine) { }
}
