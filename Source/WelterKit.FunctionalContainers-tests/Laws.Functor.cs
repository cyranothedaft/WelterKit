using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests;

internal static partial class Laws {

   internal static class Functor {
      // ===
      public static void Identity<F, A>(K<F, A> testSample,
                                        Action<K<F, A>, K<F, A>> assertAreEqual)
            where F : IFunctor<F> {
         assertAreEqual(testSample, F.FMap(testSample, id));
      }


      // fmap (g . h) = (fmap g) . (fmap h)
      public static void Composition<F, A, B,C>(K<F, A> testSample, Func<B, C> gSample, Func<A, B> hSample,
                                                Action<K<F, C>, K<F, C>> assertAreEqual)
            where F : IFunctor<F> {
         Func<K<F, A>, K<F, C>> func1 = (K<F, A> i) => F.FMap(i, Fn.Compose(gSample, hSample));
         Func<K<F, A>, K<F, C>> func2 = Fn.Compose((K<F, B> i) => F.FMap(i, gSample),
                                                   (K<F, A> j) => F.FMap(j, hSample));

         assertAreEqual(func1(testSample),
                        func2(testSample));
      }


   }

}
