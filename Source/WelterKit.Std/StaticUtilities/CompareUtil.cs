using System;
using System.Collections.Generic;
using System.Linq;



namespace WelterKit.Std.StaticUtilities {
   public static class CompareUtil {
      /// <summary>
      /// Handles comparison if one or both operands is null, by setting the value of c.
      /// Returns true if handled, false otherwise.
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="c"></param>
      /// <returns></returns>
      public static bool CompareNulls(object x, object y, ref int c) {
         // TODO: consider making return type tuple of bool,int
              if ( x == null && y == null ) c = 0;
         else if ( x == null && y != null ) c = -1;
         else if ( x != null && y == null ) c = 1;
         else
            return false;
         return true;
      }


      public static bool CompareNulls_EnsureEqual(object x, object y) {
         bool xIsNull = ( x == null ),
              yIsNull = ( y == null );
         if ( xIsNull && !yIsNull ) throw new WelterKitNotEqualException($"x != null && y == null");
         if ( !xIsNull && yIsNull ) throw new WelterKitNotEqualException($"x == null && y != null");
         return ( xIsNull && yIsNull );
      }


      public static int CompareEnum<TEnum>(TEnum x, TEnum y) where TEnum : struct, IConvertible, IComparable, IFormattable {
         // TODO: ignore over/underflows
         return System.Convert.ToInt32(x) - System.Convert.ToInt32(y);
      }


      public static int Compare(bool x, bool y) => Compare(x ? 1 : 0, y ? 1 : 0);


      public static int Compare(byte x, byte y) => Compare(( int )x, ( int )y);


      public static int Compare(int x, int y) {
         // TODO: consider using subtraction and signum()
         return ( x == y ) ? 0 : ( ( x > y ) ? 1 : -1 );
      }


      public static int Compare(long x, long y) {
         // TODO: consider using subtraction and signum()
         return ( x == y ) ? 0 : ( ( x > y ) ? 1 : -1 );
      }


      public static int Compare(DateTime x, DateTime y) {
         int c = CompareEnum(x.Kind, y.Kind);
         if ( c == 0 ) c = Compare(x.Ticks, y.Ticks);
         return c;
      }


      public static int Compare(Type x, Type y)
         => ( x == y ) ? 0 : -1;


      public static int Compare((string, string) x, (string, string) y) {
         int c = 0;
         if ( !CompareNulls(x, y, ref c) )
            c = x.CompareTo(y);
         return c;
      }


      public static int CompareSequence<T>(IList<T> x, IList<T> y, Func<T, T, int> compare)
         => compareSequenceInternal(x, y, compare, out _);


      public static void EnsureEqual_Sequence<T>(IList<T> x, IList<T> y, Func<T, T, int> compare, Func<T, string> elemToString) {
         if ( !CompareUtil.CompareNulls_EnsureEqual(x, y) ) {
            if ( compareSequenceInternal(x, y, compare, out int diffIndex) != 0 )
               throw new WelterKitNotEqualException($"Sequence element mismatch at index {diffIndex}:  {elemToString(x[diffIndex])}, {elemToString(y[diffIndex])}");
         }
      }


      private static int compareSequenceInternal<T>(IList<T> x, IList<T> y, Func<T, T, int> compare, out int diffIndex) {
         diffIndex = -1;
         int c = 0;
         if ( !CompareNulls(x, y, ref c) ) {
            c = Compare(x.Count, y.Count);
            if ( c == 0 ) {
               for ( int i = 0, len = x.Count; i < len; ++i ) {
                  c = compare(x[i], y[i]);
                  if ( c != 0 ) {
                     diffIndex = i;
                     break;
                  }
               }
            }
         }
         return c;
      }


      /// <summary>
      /// Compares the given sequences, treating them as sets and disregarding element order and repetition.
      /// </summary>
      public static int CompareSet<T>(IList<T> x, IList<T> y, IEqualityComparer<T> comparer)
         => compareSetInternal(x, y, comparer);


      private static int compareSetInternal<T>(IList<T> x, IList<T> y, IEqualityComparer<T> comparer) {
         int c = 0;
         if ( !CompareNulls(x, y, ref c) ) {
            // compare as set - compare elements regardless of position
            HashSet<T> hashSetX = new HashSet<T>(x, comparer),
                       hashSetY = new HashSet<T>(y, comparer);
            // TODO: ? produce a more meaningful int comparison result?
            c = hashSetX.SetEquals(hashSetY) ? 0 : 1;
         }
         return c;
      }


      public static int ComparePaths(string x, string y)
         => string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);


      public static bool PathsEqual(string x, string y)
         => string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);


      public static (IEnumerable<T>, IEnumerable<T>) GetSetDiffs<T>(List<T> x, List<T> y,
                                                                    IEqualityComparer<T> comparer) {
         // TODO: optimize - maybe use dynamic programming string-diff technique found recently
         IEnumerable<T> x_y = x.Except(y, comparer), // extra in x, missing from y
                        y_x = y.Except(x, comparer); // extra in y, missing from x
         return ( x_y, y_x );
      }


      public static List<(List<T>, List<T>)> GetSetDiffs<T>(List<T> x, List<T> y, List<T> z,
                                                            IEqualityComparer<T> comparer) {
         // TODO: optimize - maybe use dynamic programming string-diff technique found recently
         List<T> xy = x.Union(y, comparer).ToList(),
                 xz = x.Union(z, comparer).ToList(),
                 yz = y.Union(z, comparer).ToList();
         List<T> extraInX = x.Except(yz, comparer).ToList(),
                 extraInY = y.Except(xz, comparer).ToList(),
                 extraInZ = z.Except(xy, comparer).ToList(),
                 missingFromX = yz.Except(x, comparer).ToList(),
                 missingFromY = xz.Except(y, comparer).ToList(),
                 missingFromZ = xy.Except(z, comparer).ToList();
         return new List<(List<T>, List<T>)>()
                      {
                            ( extraInX, missingFromX ),
                            ( extraInY, missingFromY ),
                            ( extraInZ, missingFromZ ),
                      };
      }


      public sealed class IntComparer : IEqualityComparer<int>,
                                        IComparer<int> {
         bool IEqualityComparer<int>.Equals(int x, int y) => x.Equals(y);
         int IComparer<int>.Compare(int x, int y) => x.CompareTo(y);
         int IEqualityComparer<int>.GetHashCode(int x) => x.GetHashCode();
      }



      public sealed class TupleComparer : IEqualityComparer<(string, string)>,
                                          IComparer<(string, string)> {
         bool IEqualityComparer<(string, string)>.Equals((string, string) x, (string, string) y) => Compare(x, y) == 0;
         int IComparer<(string, string)>.Compare((string, string) x, (string, string) y) => Compare(x, y);
         int IEqualityComparer<(string, string)>.GetHashCode((string, string) x) => x.GetHashCode();
      }
   }
}
