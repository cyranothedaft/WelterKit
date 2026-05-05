using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Functor {
      // ===
      public static void Identity<F, A>(K<F, A> testSample,
                                        Action<K<F, A>, K<F, A>> assertAreEqual)
            where F : IFunctor<F> {
         assertAreEqual(testSample, F.FMap(testSample, id));
      }


      // fmap (g . h) = (fmap g) . (fmap h)
      public static void Composition<F, A, B, C>(K<F, A> testSample, Func<B, C> g, Func<A, B> h,
                                                 Action<K<F, C>, K<F, C>> assertAreEqual)
            where F : IFunctor<F> {

         Func<K<F, A>, K<F, C>> lhs = (K<F, A> i) => F.FMap(i, Fn.Compose(g, h));
         Func<K<F, A>, K<F, C>> rhs = Fn.Compose((K<F, B> i) => F.FMap(i, g),
                                                 (K<F, A> j) => F.FMap(j, h));

         assertAreEqual(lhs(testSample),
                        rhs(testSample));
      }
   }
}
