using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using WelterKit.Std.Functional;


namespace ConfigurationSample;

internal static class Program {

   private const LogLevel MinimumLogLevel = LogLevel.Trace;

   private const string AppSettingsDir      = ".";
   private const string AppSettingsFileName = "sample.json";


   static void Main(string[] args) {
      // seed with this explicitly settings file for this sample
      File.WriteAllText(Path.Combine(AppSettingsDir, AppSettingsFileName),
                        """
                        {
                        "key1": "file-value1",
                        "key3": "file-value3"
                        }
                        """);


      using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                                                                      builder.AddSimpleConsole(options => {
                                                                                                  options.IncludeScopes   = true;
                                                                                                  options.SingleLine      = true;
                                                                                                  options.TimestampFormat = "HH:mm:ss ";
                                                                                                  options.ColorBehavior   = LoggerColorBehavior.Enabled;
                                                                                               })
                                                                             .SetMinimumLevel(MinimumLogLevel));

      static Maybe<string> fileReader() {
         string appSettingsFilePath = Path.Combine(AppSettingsDir, AppSettingsFileName);
         return File.Exists(appSettingsFilePath)
                      ? File.ReadAllText(appSettingsFilePath)
                      : None.Value;
      }

      const string EnvPrefix = "TEST_";

      ILogger logger = loggerFactory.CreateLogger("");
      var appSettings = AppSettings.GetSettings(logger, EnvPrefix, fileReader,
                                                "key1", "key2", "key3", "keyX");

      Console.WriteLine("Effective settings:");
      foreach ((string key, string? value, SettingSource source) appSetting in appSettings) {
         Console.WriteLine(appSetting);
      }

   }


}
