using System;
using System.Collections.Generic;
using System.IO;



namespace WelterKit.Std.StaticUtilities {
   public static class StreamUtil {
      public static IEnumerable<byte> EnumBytes(this Stream stream) {
         bool eos = false;
         while ( !eos ) {
            int b = stream.ReadByte();
            eos = b == -1;
            if ( !eos )
               yield return ( byte )b;
         }
      }
   }
}
