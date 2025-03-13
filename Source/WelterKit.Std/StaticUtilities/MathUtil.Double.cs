using System;



namespace WelterKit.Std.StaticUtilities {
   public static partial class MathUtil {
      public static double ScaleRange(this double number, (double min, double max) fromRange, (double min, double max) toRange) {
         if ( toRange.min.IsApproximatelyEqualTo(toRange.max) ) return toRange.min;

         return ( number - fromRange.min )
                * getRangeScalar(fromRange, toRange)
                + toRange.min;
      }


      private static double getRangeScalar((double min, double max) fromRange, (double min, double max) toRange)
         => ( toRange.max - toRange.min ) / ( fromRange.max - fromRange.min );


      public static bool IsApproximatelyEqualTo(this double x, double y, double absoluteTolerance = double.Epsilon)
         => ( y - x ) <= absoluteTolerance;
   }
}
