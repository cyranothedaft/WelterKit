using System;

namespace WelterKit.Std.StaticUtilities ;

public static partial class MathUtil {
   /// <summary>
   /// Uses modular arithmetic to 'wrap' the given integer
   /// into the given range.
   /// </summary>
   public static int ReduceToRange(this int num, int min, int max)
      => num >= min && num <= max
               ? num
               : (num % (max - min + 1)) + min;
}
