using System;
using System.Collections.Generic;



namespace WelterKit.ClassTools {
   public class GeneralComparator<T> : IEqualityComparer<T> {
      private readonly Func<T, T, bool> _equalsFunc;
      private readonly Func<T, int> _getHashCodeFunc;


      public GeneralComparator(Func<T, T, bool> equalsFunc, Func<T, int> getHashCodeFunc) {
         _equalsFunc      = equalsFunc;
         _getHashCodeFunc = getHashCodeFunc;
      }


      bool IEqualityComparer<T>.Equals(T x, T y) => _equalsFunc(x, y);
      int IEqualityComparer<T>.GetHashCode(T obj) => _getHashCodeFunc(obj);
   }
}
