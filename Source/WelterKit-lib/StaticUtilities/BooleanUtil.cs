using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WelterKit.StaticUtilities {
   public static class BooleanUtil {
      public static string ToYN(this bool b) => b ? "Y" : "N";
   }
}
