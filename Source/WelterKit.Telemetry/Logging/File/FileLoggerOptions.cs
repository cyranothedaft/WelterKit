using System;


namespace WelterKit.Telemetry.Logging.File;

public class FileLoggerOptions {
   public string? LogFilePath { get; set; } = null;
   public bool ForceSingleLine { get; set; } = false;
}
