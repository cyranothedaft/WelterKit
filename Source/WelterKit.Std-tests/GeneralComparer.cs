using System;
using System.Collections;
using System.Collections.Generic;



namespace WelterKit.Std_Tests {
   // TODO: move to WelterKit.Testing
   public class GeneralComparer<T> : IComparer<T>, IComparer {
      private readonly Func<T, T, int> _compareFunc;

      public GeneralComparer(Func<T, T, int> compareFunc) {
         _compareFunc = compareFunc;
      }

      public int Compare(T x, T y)
         => _compareFunc(x, y);

      public int Compare(object x, object y)
         => _compareFunc(( T )x, ( T )y);
   }
}
