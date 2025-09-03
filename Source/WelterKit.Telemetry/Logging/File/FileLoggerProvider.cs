using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;



namespace WelterKit.Telemetry.Logging.File;

public class FileLoggerProvider : ILoggerProvider, ISupportExternalScope {
   public const bool Default_ForceSingleLine = false;
   
   private StreamWriter? _streamWriter = null;
   private IExternalScopeProvider? _scopeProvider = null;


   internal FileLoggerOptions? Settings { get; private set; }


   // protected IDisposable SettingsChangeToken;


   public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> Settings)
         : this(Settings.CurrentValue) {
      // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/change-tokens
      // TODO?
      // SettingsChangeToken = Settings.OnChange(settings => { this.Settings = settings; });
   }


   public FileLoggerProvider(FileLoggerOptions settings) {
      Settings = settings;
   }


   public void Dispose() {
      _streamWriter?.Dispose();
   }


   internal IExternalScopeProvider ScopeProvider
      => _scopeProvider ?? new LoggerExternalScopeProvider();


   public ILogger CreateLogger(string categoryName) {
      // create new file stream that will be disposed by Dispose()
      _streamWriter ??= System.IO.File.AppendText(Settings?.LogFilePath
                                               ?? throw new Exception("Cannot create log file stream - Settings?.LogFilePath is null"));
      FileLogger logger = new(categoryName, ScopeProvider, isEnabled, _streamWriter,
                              Settings?.ForceSingleLine ?? Default_ForceSingleLine);
      logger.LogInformation("Logger created");
      return logger;
   }


   private bool isEnabled(LogLevel logLevel)
      => true;


   void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider? scopeProvider) {
      _scopeProvider = scopeProvider;
   }
}
