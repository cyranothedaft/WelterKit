using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Monad {
      // Left identity:   return a  >>= h  ≡  h a
      public static void LeftIdentity<M, A, B>(A a,
                                               Func<A, K<M, B>> h,
                                               Action<K<M, B>, K<M, B>> assertAreEqual)
            where M : IMonad<M> {

         K<M, B> lhs = M.Bind(M.Return(a), h);
         K<M, B> rhs = h(a);

         assertAreEqual(lhs, rhs);
      }


      // Right identity:  m >>= return  ≡  m
      public static void RightIdentity<M, A>(K<M, A> ma,
                                             Action<K<M, A>, K<M, A>> assertAreEqual)
            where M : IMonad<M> {

         K<M, A> lhs = M.Bind(ma, M.Return);
         K<M, A> rhs = ma;

         assertAreEqual(lhs, rhs);
      }


      // Associativity:   (m >>= g) >>= h  ≡  m >>= (\x -> g x >>= h)
      public static void Associativity<M, A, B>(K<M,A> ma,
                                               Func<A, K<M, B>> g,
                                               Func<B, K<M, B>> h,  // <-- TODO: confirm this is one is the correct type
                                               Action<K<M, B>, K<M, B>> assertAreEqual)
            where M : IMonad<M> {
         
         var lhs = ma.Bind(g).Bind(h);
         var rhs = ma.Bind(x => g(x).Bind(h));

         assertAreEqual(lhs, rhs);
      }
   }
}
