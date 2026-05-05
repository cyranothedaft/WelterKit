using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Applicative{

      // Identity:  pure id <*> v  =  v
      public static void Identity<F, A>(K<F, A> v,
                                        Action<K<F, A>, K<F, A>> assertAreEqual)
            where F : IApplicative<F> {
         assertAreEqual(F.Apply(F.Pure<Func<A, A>>(id), v),
                        v);
      }


      // Composition:  pure (.) <*> u <*> v <*> w  =  u <*> (v <*> w)
      public static void Composition<F, A, B, C>(K<F, Func<B, C>> u,
                                                 K<F, Func<A, B>> v,
                                                 K<F, A> w,
                                                 Action<K<F, C>, K<F, C>> assertAreEqual)
            where F : IApplicative<F> {
         var composeCurried = Fn.Curry<Func<B, C>, Func<A, B>, Func<A, C>>(Fn.Compose);
         var a = F.Pure(composeCurried);

         K<F, C> x = a.Apply(u)
                      .Apply(v)
                      .Apply(w);

         K<F, C> y = u.Apply(v.Apply(w));

         assertAreEqual(x, y);
      }


      // Homomorphism:  pure f <*> pure x  =  pure (f x)
      public static void Homomorphism<F, A, B>(A x,
                                               Func<A, B> f,
                                               Action<K<F, B>, K<F, B>> assertAreEqual)
            where F : IApplicative<F> {

         K<F, B> lhs = F.Apply(F.Pure(f),
                               F.Pure(x));
         K<F, B> rhs = F.Pure(f(x));

         assertAreEqual(lhs, rhs);
      }


      // Interchange:  u <*> pure y  =  pure ($ y) <*> u
      public static void Interchange<F, A, B>(K<F, Func<A, B>> u,
                                              A y,
                                              Action<K<F, B>, K<F, B>> assertAreEqual)
            where F : IApplicative<F> {

         K<F, B> lhs = F.Apply(u, F.Pure(y));
         K<F, B> rhs = F.Pure(((Func<A, B> x) => x(y)))
                        .Apply(u);

         assertAreEqual(lhs, rhs);
      }

   }

}
