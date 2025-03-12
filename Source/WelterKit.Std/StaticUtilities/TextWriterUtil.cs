using System;
using System.IO;



namespace WelterKit.Std.StaticUtilities {
   public static class TextWriterUtil {
      public static string GetString(this Action<TextWriter> action) {
         using ( var textWriter = new StringWriter() ) {
            action(textWriter);
            return textWriter.ToString();
         }
      }
   }
}
