using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using WelterKit.Telemetry.Logging.File;


namespace LoggingSample;

internal static class Program {
   static void Main(string[] args) {
      using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
                                                             builder
                                                                  .AddSimpleConsole(options => {
                                                                                       options.IncludeScopes   = true;
                                                                                       options.SingleLine      = true;
                                                                                       options.TimestampFormat = "HH:mm:ss ";
                                                                                       options.ColorBehavior   = LoggerColorBehavior.Enabled;
                                                                                    })

                                                                  .AddFile((MinimalFileLoggerOptions options) => {
                                                                              options.LogFilePath     = @".\sample.log";
                                                                              options.ForceSingleLine = true;
                                                                           })

                                                                  .AddFilter("Program", LogLevel.Information)
                                                                  .SetMinimumLevel(LogLevel.Trace) // fallback/default
                                                                   ;
                                                          });

      ILogger programLogger = loggerFactory.CreateLogger(nameof(Program));

      programLogger.LogTrace      ("Main - Trace      ");
      programLogger.LogDebug      ("Main - Debug      ");
      programLogger.LogInformation("Main - Information");
      programLogger.LogWarning    ("Main - Warning    ");
      programLogger.LogError      ("Main - Error      ");
      programLogger.LogCritical   ("Main - Critical   ");

      using ( programLogger.BeginScope("subscope-1") ) {
         programLogger.LogTrace      ("Main-sub1 - Trace      ");
         programLogger.LogDebug      ("Main-sub1 - Debug      ");
         programLogger.LogInformation("Main-sub1 - Information");
         programLogger.LogWarning    ("Main-sub1 - Warning    ");
         programLogger.LogError      ("Main-sub1 - Error      ");
         programLogger.LogCritical   ("Main-sub1 - Critical   ");

         using ( programLogger.BeginScope("subscope-2") ) {
            programLogger.LogTrace      ("Main-sub2 - Trace      ");
            programLogger.LogDebug      ("Main-sub2 - Debug      ");
            programLogger.LogInformation("Main-sub2 - Information");
            programLogger.LogWarning    ("Main-sub2 - Warning    ");
            programLogger.LogError      ("Main-sub2 - Error      ");
            programLogger.LogCritical   ("Main-sub2 - Critical   ");
         }

         ILogger auxLogger = loggerFactory.CreateLogger("Auxiliary");
         auxLogger.LogTrace      ("Main-aux - Trace      ");
         auxLogger.LogDebug      ("Main-aux - Debug      ");
         auxLogger.LogInformation("Main-aux - Information");
         auxLogger.LogWarning    ("Main-aux - Warning    ");
         auxLogger.LogError      ("Main-aux - Error      ");
         auxLogger.LogCritical   ("Main-aux - Critical   ");
      }
   }
}
