using System;
using System.Collections.Generic;
using System.Linq;



namespace WelterKit.Std.StaticUtilities {
   public static class ByteUtil {
      public static byte[] HexToByteArray(string byteStr) {
         if ( byteStr == null ) return null;
         return enumBytes(byteStr).ToArray();

         IEnumerable<byte> enumBytes(string str) {
            for ( int p = 0, len = byteStr.Length; p < len; p += 2 ) {
               string bStr = str.Substring(p, 2);
               byte b = Convert.ToByte(bStr, 16);
               yield return b;
            }
         }
      }


      public static string ByteArrayToHex(byte[] bytes) {
         if ( bytes == null ) return null;
         return string.Concat(bytes.Select(b => b.ToString("x2")));
      }


      public static string ByteArrayToHexSpaced(byte[] bytes, string joinStr) {
         if ( bytes == null ) return null;
         return string.Join(joinStr, bytes.Select(b => b.ToString("x2")));
      }


      public static int CompareByteArrays(byte[] x, byte[] y) {
         if ( x == null ) throw new ArgumentNullException(nameof( x ));
         if ( y == null ) throw new ArgumentNullException(nameof( y ));
         return CompareUtil.CompareSequence(x, y, CompareUtil.Compare);
      }
   }
}
