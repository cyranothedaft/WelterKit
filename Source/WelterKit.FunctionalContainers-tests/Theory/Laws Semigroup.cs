using System;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Semigroup {
      // (a <> b) <> c == a <> (b <> c)
      public static void Associativity<M>(M a, M b, M c,
                                          Action<M, M> assertAreEqual)
            where M : ISemigroup<M> {
         M lhs = M.Combine(M.Combine(a, b), c);
         M rhs = M.Combine(a, M.Combine(b, c));

         assertAreEqual(lhs, rhs);
      }
   }
}
