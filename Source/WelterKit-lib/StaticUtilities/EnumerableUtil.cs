using WelterKit.Functional;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.StaticUtilities {
   public static class EnumerableUtil {
      public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey, Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> result)
         => left.GroupJoin(right, leftKey, rightKey, (l, r) => new { l, r })
                .SelectMany(o => o.r.DefaultIfEmpty(), (l, r) => new { lft = l.l, rght = r })
                .Select(o => result(o.lft, o.rght));


      public static IEnumerable<(TLeft, TRight)> LeftOuterJoin<TLeft, TRight, TKey>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey, Func<TRight, TKey> rightKey)
         => ( left ?? Enumerable.Empty<TLeft>() )
            .GroupJoin(right ?? Enumerable.Empty<TRight>(), leftKey, rightKey, (l, r) => ( l, r ))
            .SelectMany(o => o.Item2.DefaultIfEmpty(), (l, r) => ( l.Item1, r ));


      public static IEnumerable<(TLeft, TRight)> FullOuterJoin<TLeft, TRight, TKey>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey, Func<TRight, TKey> rightKey,
            IEqualityComparer<(TLeft, TRight)> comparer) {
         var joinLeft = LeftOuterJoin(left, right, leftKey, rightKey);
         var joinRight = LeftOuterJoin(right, left, rightKey, leftKey)
               .Select(x => ( x.Item2, x.Item1 ));
         return joinLeft.Union(joinRight, comparer);
      }


      // TODO: test
      public static IEnumerable<(int index, TResult value)> Indexed<TResult>(this IEnumerable<TResult> source)
         => source.Select((x, i) => ( index: i, value: x ));


      public static IEnumerable<(T, T)> Chain<T>(this IEnumerable<T> source) {
         bool isFirst = true;
         T prevElem = default( T );
         foreach ( T elem in source ) {
            if ( isFirst ) {
               isFirst = false;
            }
            else {
               yield return ( prevElem, elem );
            }
            prevElem = elem;
         }
      }


      // TODO: get this to work with Either<IEnumerable<T>,T>
      public static IEnumerable<T> ConcatParts<T>(this IEnumerable<object> parts) {
         foreach ( object part in parts ) {
            switch ( part ) {
               case IEnumerable<T> sequence:
                  foreach ( T item in sequence )
                     yield return item;
                  break;

               case T item:
                  yield return item;
                  break;

               default:
                  yield return ( T )System.Convert.ChangeType(part, typeof( T ));
                  break;
            }
         }
      }


      public static IEnumerable<T> ConcatParts<T>(params object[] parts)
         => ConcatParts<T>(( IEnumerable<object> )parts);


      public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
         => source.TryFirst(predicate, out T item)
                  ? ( Maybe<T> )new Some<T>(item)
                  : ( Maybe<T> )new None<T>();


      public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source)
         => source.FirstOrNone(_ => true);


      public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
         => source.SelectMany(x => x);


      public static IEnumerable<T> Repeat<T>(this T item, int count)
         => Enumerable.Range(0, count).Select(_ => item);


      // TODO: test more
      public static IEnumerable<T> ReplaceAll<T>(this IEnumerable<T> source, Func<T, bool> predicateFunc, T replaceWith) {
         foreach ( T x in source ) {
            yield return predicateFunc(x)
                            ? replaceWith
                            : x;
         }
      }


      // TODO: test
      public static IEnumerable<TResult> SelectOptional<T, TResult>(this IEnumerable<T> sequence, Func<T, Maybe<TResult>> map)
         => sequence.Select(map)
                    .OfType<Some<TResult>>()
                    .Select(some => ( TResult )some);


      // TODO: test
      public static IEnumerable<T> SelectOptional<T>(this IEnumerable<Maybe<T>> sequence)
         => sequence.OfType<Some<T>>()
                    .Select(some => ( T )some);


      // TODO: test
      public static bool TryFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T matchedElement) {
         foreach ( T element in source ) {
            if ( predicate(element) ) {
               matchedElement = element;
               return true;
            }
         }
         matchedElement = default( T );
         return false;
      }


      /// <remarks>Reportedly faster than a single 'yield return'.</remarks>
      public static IEnumerable<T> Yield<T>(this T value)
         => new[] { value };


      public static IEnumerable<TResult> ZipToEnd<T1, T2, TResult>(this IEnumerable<T1> sequence1, IEnumerable<T2> sequence2,
                                                                   Func<(T1 element1, T2 element2, bool isSequence1Ended, bool isSequence2Ended), TResult> resultSelector,
                                                                   T1 default1, T2 default2) {
         using IEnumerator<T1> enumerator1 = sequence1.GetEnumerator();
         using IEnumerator<T2> enumerator2 = sequence2.GetEnumerator();

         bool advanced1 = true,
              advanced2 = true;
         while ( true ) {
            advanced1 = advanced1 && enumerator1.MoveNext();
            advanced2 = advanced2 && enumerator2.MoveNext();
            var item = ( advanced1 ? enumerator1.Current : default1,
                         advanced2 ? enumerator2.Current : default2,
                         !advanced1,
                         !advanced2 );

            // stop when both enumerations are ended
            if ( !advanced1 && !advanced2 ) break;

            yield return resultSelector(item);
         }
      }

      public static IEnumerable<TResult>                                                                  ZipToEnd<T1, T2, TResult>(this IEnumerable<T1> sequence1, IEnumerable<T2> sequence2, Func<(T1 element1, T2 element2, bool isSequence1Ended, bool isSequence2Ended), TResult> resultSelector) => ZipToEnd(sequence1, sequence2, resultSelector, default( T1 ), default( T2 ));
      public static IEnumerable<(T1 element1, T2 element2, bool isSequence1Ended, bool isSequence2Ended)> ZipToEnd<T1, T2>         (this IEnumerable<T1> sequence1, IEnumerable<T2> sequence2, T1 default1, T2 default2                                                                              ) => ZipToEnd(sequence1, sequence2, tuple => tuple, default1     , default2);
      public static IEnumerable<(T1 element1, T2 element2, bool isSequence1Ended, bool isSequence2Ended)> ZipToEnd<T1, T2>         (this IEnumerable<T1> sequence1, IEnumerable<T2> sequence2                                                                                                        ) => ZipToEnd(sequence1, sequence2, tuple => tuple, default( T1 ), default( T2 ));


      /// <summary>
      /// Note: This currently assumes the sequences have the same length
      /// </summary>
      public static IEnumerable<(T1, T2)> ZipTuples<T1, T2>(IEnumerable<T1> sequence1, IEnumerable<T2> sequence2)
         => ZipToEnd(sequence1, sequence2, tuple => (tuple.element1, tuple.element2));
   }
}
