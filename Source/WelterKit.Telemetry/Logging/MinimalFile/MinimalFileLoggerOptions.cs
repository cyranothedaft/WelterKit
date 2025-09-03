using System;


namespace WelterKit.Telemetry.Logging.MinimalFile;

public class MinimalFileLoggerOptions {
   public string? LogFilePath { get; set; } = null;
   public bool ForceSingleLine { get; set; } = false;
}
