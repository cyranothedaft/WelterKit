using System;



namespace WelterKit.Std.StaticUtilities {
   public static partial class MathUtil {
      public static float ScaleRange(this float number, (float min, float max) fromRange, (float min, float max) toRange) {
         if ( toRange.min.IsApproximatelyEqualTo(toRange.max) ) return toRange.min;

         return ( number - fromRange.min )
                * getRangeScalar(fromRange, toRange)
                + toRange.min;
      }


      private static float getRangeScalar((float min, float max) fromRange, (float min, float max) toRange)
         => ( toRange.max - toRange.min ) / ( fromRange.max - fromRange.min );


      public static bool IsApproximatelyEqualTo(this float x, float y, float absoluteTolerance = float.Epsilon)
         => ( y - x ) <= absoluteTolerance;
   }
}
