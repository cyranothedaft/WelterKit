using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Functor {
      // ===
      public static void Identity<M, A>(K<M, A> testSample,
                                        Action<K<M, A>, K<M, A>> assertAreEqual)
            where M : IFunctor<M> {
         assertAreEqual(testSample, M.FMap(testSample, id));
      }


      // fmap (g . h) = (fmap g) . (fmap h)
      public static void Composition<M, A, B, C>(K<M, A> testSample, Func<B, C> g, Func<A, B> h,
                                                 Action<K<M, C>, K<M, C>> assertAreEqual)
            where M : IFunctor<M> {

         Func<K<M, A>, K<M, C>> lhs = (K<M, A> i) => M.FMap(i, Fn.Compose(g, h));
         Func<K<M, A>, K<M, C>> rhs = Fn.Compose((K<M, B> i) => M.FMap(i, g),
                                                 (K<M, A> j) => M.FMap(j, h));

         assertAreEqual(lhs(testSample),
                        rhs(testSample));
      }
   }
}
