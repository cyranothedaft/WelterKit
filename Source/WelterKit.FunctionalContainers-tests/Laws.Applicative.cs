using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests;

internal static partial class Laws {

   internal static class Applicative{

// Identity: pure id <*> v = v
// Composition: pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
// Homomorphism: pure f <*> pure x = pure (f x)
// Interchange: u <*> pure y = pure ($ y) <*> u


      // Identity: pure id <*> v = v
      public static void Identity<F, A>(K<F, A> testSample,
                                        Action<K<F, A>, K<F, A>> assertAreEqual)
            where F : IApplicative<F> {
         assertAreEqual(F.Apply(testSample, F.Pure<Func<A, A>>(id)),
                        testSample);
      }


      // Composition: pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
      public static void Composition<F, A, B,C>(K<F, A> testSample, Func<B, C> gSample, Func<A, B> hSample,
                                                Action<K<F, C>, K<F, C>> assertAreEqual)
            where F : IApplicative<F> {

         K<F, Func<Func<B, C>, Func<Func<A, B>, Func<A, C>>>> a = F.Pure(Fn.Curry<Func<B, C>, Func<A, B>, Func<A, C>>(Fn.Compose));
         K<F, A> u;
         var b = F.Apply<A, Func<Func<A, B>, Func<A, C>>>(u,a);

      }


   }

}
