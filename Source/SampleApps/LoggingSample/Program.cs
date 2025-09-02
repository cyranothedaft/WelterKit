using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using WelterKit.Telemetry.Logging.File;



namespace LoggingSample;

internal static class Program {
   static void Main(string[] args) {
      ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
                                                             builder
                                                                  .AddSimpleConsole(options => {
                                                                                       options.IncludeScopes   = true;
                                                                                       options.SingleLine      = true;
                                                                                       options.TimestampFormat = "HH:mm:ss ";
                                                                                       options.ColorBehavior   = LoggerColorBehavior.Enabled;
                                                                                    })

                                                                  .AddFile((FileLoggerOptions options) => {
                                                                              options.LogFilePath     = @".\sample.log";
                                                                              options.ForceSingleLine = true;
                                                                           })

                                                                   // .AddFilter("program", LogLevel.Information)
                                                                  .SetMinimumLevel( LogLevel.Trace) // fallback/default
                                                                   ;
                                                          });

      ILogger programLogger = loggerFactory.CreateLogger("Program");

      programLogger.LogTrace      ("Main - Trace      ");
      programLogger.LogDebug      ("Main - Debug      ");
      programLogger.LogInformation("Main - Information");
      programLogger.LogWarning    ("Main - Warning    ");
      programLogger.LogError      ("Main - Error      ");
      programLogger.LogCritical   ("Main - Critical   ");
   }
}
