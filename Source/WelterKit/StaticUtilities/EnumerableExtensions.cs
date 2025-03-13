using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;



namespace WelterKit.StaticUtilities;

public static class EnumerableExtensions {
   public static IEnumerable<T> CoalesceNulls<T>(this IEnumerable<T?> source)
      => source.Where(x => x is not null)
               .Select(x => x!);


   public static (ImmutableList<T> list1, ImmutableList<T> list2) Partition<T>(this IEnumerable<T> source, Func<T, bool> selectorForList2) {
      ImmutableList<T>.Builder builder1 = ImmutableList.CreateBuilder<T>(),
                               builder2 = ImmutableList.CreateBuilder<T>();
      foreach ( T item in source )
         ( selectorForList2(item)
                 ? builder2
                 : builder1
         ).Add(item);
      return (builder1.ToImmutable(),
              builder2.ToImmutable());
   }


   public static (ImmutableList<TResult1> list1, ImmutableList<TResult2> list2) Separate<T, TResult1, TResult2>(this IEnumerable<T> source, Func<T, (TResult1, TResult2)> mapSplitFunc) {
      ImmutableList<TResult1>.Builder builder1 = ImmutableList.CreateBuilder<TResult1>();
      ImmutableList<TResult2>.Builder builder2 = ImmutableList.CreateBuilder<TResult2>();

      foreach ( T element in source ) {
         ( TResult1 item1, TResult2 item2 ) = mapSplitFunc(element);
         builder1.Add(item1);
         builder2.Add(item2);
      }

      return ( builder1.ToImmutableList(),
               builder2.ToImmutableList() );
   }
}
