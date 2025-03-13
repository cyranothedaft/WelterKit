using System;
using System.Collections.Generic;
using System.Linq;



namespace WelterKit.Std.Sorting {
   public static class MergeSort {
      internal static List<T> Merge<T>(IList<T> a, IList<T> b, Func<T, T, int> compareFunc)
         => mergeEnum(a, b, compareFunc).ToList();


      private static IEnumerable<T> mergeEnum<T>(IList<T> a, IList<T> b, Func<T, T, int> compareFunc) {
         int aLen = a.Count,
             bLen = b.Count;
         int i = 0,
             j = 0;
         while ( i < aLen && j < bLen )
            yield return lessOrEqual(a[i], b[j])
                            ? a[i++]
                            : b[j++];
         while ( i < aLen )
            yield return a[i++];
         while ( j < bLen )
            yield return b[j++];

         bool lessOrEqual(T x, T y)
            => compareFunc(x, y) <= 0;
      }


      public static ICollection<T> Sort<T>(IList<T> list, Func<T, T, int> compareFunc) {
         IEnumerable<DSpan<T>> divided = DivideAll(list); 
throw new NotImplementedException();
      }


      internal static IEnumerable<DSpan<T>> DivideAll<T>(IList<T> list) {
         throw new NotImplementedException();
      }


      internal static IEnumerable<DSpan<T>> Divide<T>(DSpan<T> list) {
         if ( list.Length == 0 )
            yield break;
         else if ( list.Length == 1 )
            yield return list;
         else if ( list.Length % 2 == 0 ) {
            yield return list.Slice(0, list.Length / 2);
            yield return list.Slice(( list.Length / 2 ) + 1);
         }
         else {
            yield return list.Slice(0, list.Length / 2 + 1);
            yield return list.Slice(( list.Length / 2 ) + 2);
         }
      }
   }
}
