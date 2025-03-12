using System;



namespace WelterKit.Std.StaticUtilities {
   public static class TupleUtil {
      public static (string, string) ToTuple(this string[] array) {
         if ( array == null ) throw new ArgumentNullException(nameof( array ));
         if ( array.Length != 2 ) throw new ArgumentException($"Require array of length 2; this array has length {array.Length}.", nameof( array ));
         return ( array[0], array[1] );
      }


      public static (TResult1, TResult2) Map2<T, TResult1, TResult2>(this T obj, Func<T, TResult1> map1, Func<T, TResult2> map2)
         => (map1(obj), map2(obj));


      public static (T1, T2) MapThrough<T1, T2>(this (T1, T2) tuple, Action<T1, T2> action) {
         action(tuple.Item1, tuple.Item2);
         return tuple;
      }
   }
}
