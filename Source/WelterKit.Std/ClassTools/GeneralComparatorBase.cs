using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std.ClassTools {
   public abstract class GeneralComparatorBase<T> : IEqualityComparer, IEqualityComparer<T>,
                                                    IComparer, IComparer<T>
                                                    where T : TooledClassBase {
      protected abstract bool IsNullable { get; }

      protected abstract IEnumerable<Func<T, T, int>> GetOrderedCompareOps();

      // loaded on first use
      private Func<T, T, int>[] _orderedCompareOps;
      // ideally, this would be private, but some implementations have constructors that rely on parent base class implementations
      public Func<T, T, int>[] OrderedCompareOps
         => _orderedCompareOps ?? ( _orderedCompareOps = GetOrderedCompareOps().ToArray() );


      public bool Equals(T x, T y) => Compare(x, y) == 0;


      // TODO: include EnsureEquals()


      public int Compare(T x, T y) {
         return IsNullable
                      ? compareNullable(x, y)
                      : compare(x, y);
      }


      private int compare(T x, T y) {
         return OrderedCompareOps
                .Select(compareOp => compareOp(x, y))
                .FirstOrDefault(c => c != 0);
      }

      private int compareNullable(T x, T y) {
         int c = 0;
         if ( !CompareUtil.CompareNulls(x, y, ref c) ) {
            c = OrderedCompareOps
                .Select(compareOp => compareOp(x, y))
                .FirstOrDefault(cc => cc != 0);
         }
         return c;
      }


      int IComparer.Compare(object x, object y) => this.Compare(x as T, y as T);
      int IComparer<T>.Compare(T x, T y) => this.Compare(x, y);

      bool IEqualityComparer.Equals(object x, object y) => this.Equals(x as T, y as T);
      bool IEqualityComparer<T>.Equals(T x, T y) => this.Equals(x, y);

      int IEqualityComparer.GetHashCode(object obj) => ( ( T )obj ).GetHashCode();
      int IEqualityComparer<T>.GetHashCode(T obj) => obj.GetHashCode();
   }
}
