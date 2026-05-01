using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Semigroup {
      // (a <> b) <> c == a <> (b <> c)
      public static void Associativity<F, A>(K<F, A> a, K<F, A> b, K<F, A> c,
                                             Action<K<F, A>, K<F, A>> assertAreEqual)
            where F : ISemigroup<F> {
         K<F, A> lhs = F.Combine(F.Combine(a, b), c);
         K<F, A> rhs = F.Combine(a, F.Combine(b, c));

         assertAreEqual(lhs, rhs);
      }
   }
}
