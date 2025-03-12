using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace WelterKit.StaticUtilities {
   public static class TextReaderUtil {
      public static IEnumerable<string> ReadLinesToEnd(this TextReader reader, params string[] alsoStopAt)
         => reader.ReadLinesToEnd(( IEnumerable<string> )alsoStopAt);


      public static IEnumerable<string> ReadLinesToEnd(this TextReader reader, IEnumerable<string>? alsoStopAt = null) {
         HashSet<string> stopAt = new HashSet<string>(alsoStopAt ?? Enumerable.Empty<string>());
         bool anyStopAt = stopAt.Any();

         string? line;
         while ( !isDone(line = reader.ReadLine()) ) {
            yield return line!;
         }

         bool isDone(string? str)
            => str is null
            || anyStopAt && stopAt!.Contains(str);
      }
   }
}
