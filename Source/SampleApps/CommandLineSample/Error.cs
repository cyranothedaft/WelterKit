using System;
using WelterKit.Std;


namespace CommandLineSample;


internal record Error(string DisplayText) : IError;


internal record ExceptionError(Exception Exception, string WhileDoing) : Error(format(Exception, WhileDoing)) {
   private static string format(Exception exception, string whileDoing)
      => $"Error [{exception.GetType().Name}] while [{whileDoing}]: {exception.Message}";
}
