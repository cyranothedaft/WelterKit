/* TODO
using System;
using System.Collections.Generic;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Traversible {

      // Identity: traverse Identity = Identity (using the Identity applicative wrapper).
      // Composition: traverse (Compose . fmap g . f) = Compose . fmap (traverse g) . traverse f.

      // t . traverse f == traverse (t . f) -- for every applicative transformer t
      // i.e.:
      // t (traverse f x) = traverse (\y -> t (f y)) x
      //
      public static void Naturality<T, A, B, F, G,H>(Func<A, K<G, B>> f,
                                                   K<T, A> a,
                                                   Func<K<G, K<T, B>>, K<H, K<T, B>>> n, // natural transformation
                                                   Action<K<H, K<T, B>>, T> assertAreEqual) where T : ITraversible<T>
                                                                                            where G : IApplicative<G>
                                                                                            where H : IApplicative<H>
      {
         // K<
         // var lhs = Fn.Compose()
         // assertAreEqual(lhs,rhs);


         K<G, K<T, B>> traversed1 = T.Traverse(f, a);
         K<H, K<T, B>> lhs = n(traversed1);

//         Func<Func<A, K<G, B>>,
//              K<T, A>,
//              K<G, K<T, B>>
//         > traverse = T.Traverse<G,A,B>;
//         var traverseCurried = Fn.Curry(traverse);
//
//         // var lhs = Fn.Compose(n, traverseCurried);
//         var lhs = (Func<A, K<G, B>> arg) => {
//                      Func<K<T, A>, K<G, K<T, B>>> first = traverseCurried(arg);
//                      n()
//                   };
//
//         // var n_f = Fn.Compose<A, K<G, K<T, B>>, K<H, K<T, B>>>(n, f);
//          var t_f = (A x) => n(f(x));

          void rf(A y) {
             K<G, B> _1 = f(y);
             K<G, K<T, B>> exp;
             var _2 = n(exp);
          }

         assertAreEqual(lhs, rhs);
      }
   }
}
*/