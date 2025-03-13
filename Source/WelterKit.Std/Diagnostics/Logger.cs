using System;
using System.IO;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std.Diagnostics {
   public partial class Logger {
      private static Logger _instance;
      public static Logger L => _instance;

      private static readonly string SeparatorText = new string('=', 79);

      // === TODO: considering moving these into CLI project
      private const string LogFileDir = @".\";
      //private const string LogFileDir = @"..\..";
      private const string LogFileNameFmt = @"sync4me_{0}.log";


      private readonly TextWriter _out;
      private readonly string _logFilePath;


      private Logger(string logFilePath) {
         _out = null;
         _logFilePath = logFilePath;
      }


      private Logger(TextWriter @out) {
         _out = @out;
         _logFilePath = null;
      }


      public static void Init_File(AppInfo appInfo = null, string logDir = null, string logFileName = null) {
         AppInfo actualAppInfo = appInfo ?? new AppInfo();
         string actualLogFileDir = logDir ?? LogFileDir,
                actualLogFileName = logFileName ?? string.Format(LogFileNameFmt, "debug"),
                actualLogFilePath = Path.Combine(actualLogFileDir, actualLogFileName);
         //_logFilePath = Path.Combine(LogFileDir, string.Format(LogFileNameFmt, DateTime.Now.ToString("yyyyMMdd-HHmmss")));
         File.Delete(actualLogFilePath);
         _instance = new Logger(actualLogFilePath);
         postInit(DiagStr.StringInfo(FileSystemUtil.ResolveFullPath(_instance._logFilePath)), actualAppInfo);
      }


      public static void Init_TextWriter(AppInfo appInfo = null, TextWriter @out = null) {
         AppInfo actualAppInfo = appInfo ?? new AppInfo();
         TextWriter actualOut = @out ?? Console.Out;
         _instance = new Logger(actualOut);
         postInit(@out?.GetType().Name, actualAppInfo);
      }


      private static void postInit(string targetDesc, AppInfo appInfo) {
         _instance.writeSeparator();
         _instance.writeMsgWithTime($"*** Initiated log - {appInfo.Title} version: {appInfo.Version}\r\n"
                                    + $"Writing to: {targetDesc}");
      }


      public void Write(string msg) {
         writeMsg(msg);
      }


      public void WriteSeparated(string msg) {
         writeMsg(SeparatorText);
         writeMsg(msg);
         writeMsg(SeparatorText);
      }


      //public void Write(Func<string> msgFunc) {
      //   writeMsg(msgFunc());
      //}


      public void WriteWithTime(string msg, bool includeSeparator) {
         if ( includeSeparator )
            writeSeparator();
         writeMsgWithTime(msg);
      }


      public void WriteException(Exception exception) {
         writeMsgWithTime("ERROR:\r\n" + exception.ToString());
      }


      public void WriteMethodCall(string className, string methodName) {
         Write($"{className}.{methodName}() - called");
      }


      public void WriteMethodCall<TParams>(string className, string methodName, DebugInfoBase<TParams> paramInfo) {
         Write(DiagStr.Label($"{className}.{methodName}() - called", paramInfo, indentOverride: 4));
      }


      //public void WriteMethodCall(string className, string methodName, params (string, DebugInfoBase)[] paramNamedValues) {
      //   Write(paramNamedValues.Any()
      //               ? $"{className}.{methodName}() - called:\r\n" + new DebugInfoLabelledItems(paramNamedValues).RenderAsString("   ")
      //               : $"{className}.{methodName}() - called");
      //}


      public void WriteMethodReturn(string className, string methodName) {
         Write($"{className}.{methodName}() - returning");
      }


      public void WriteMethodReturn<TReturn>(string className, string methodName, DebugInfoBase<TReturn> returnInfo) {
         Write(DiagStr.Label($"{className}.{methodName}() - returning", returnInfo, indentOverride: 4));
      }


      private void writeMsgWithTime(string msg) {
         writeMsg($"\r\n" +
                  $"*** [{DateTime.Now:O}]\r\n" +
                  $"{msg}");
      }


      private void writeSeparator() {
         writeMsg("\r\n" + SeparatorText);
      }


      private void writeMsg(string msg) {
         if ( _out != null )
            _out.WriteLine(msg);
         else
            File.AppendAllText(_logFilePath, msg + "\r\n");
      }
   }
}
