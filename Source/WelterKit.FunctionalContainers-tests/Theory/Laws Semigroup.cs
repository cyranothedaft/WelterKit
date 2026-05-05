using System;
using System.Collections.Generic;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Semigroup {
      // (a <> b) <> c == a <> (b <> c)
      public static void Associativity<M>((M a, M b, M c) triplet,
                                          Action<M, M> assertAreEqual) where M : ISemigroup<M> {
         var (a, b, c) = triplet;

         //public static void Associativity<M>(M a, M b, M c,
         //                                    Action<M, M> assertAreEqual) where M : ISemigroup<M> {

         M lhs = M.Combine(M.Combine(a, b), c);
         M rhs = M.Combine(a, M.Combine(b, c));

         assertAreEqual(lhs, rhs);
      }


      // // partial function application
      // public static Action<M, M, M> Associativity<M>(Action<M, M> assertAreEqual) where M : ISemigroup<M>
      //    => (a, b, c) => Associativity(a, b, c, assertAreEqual);


      public static void AssociativityMulti<M>(IEnumerable<(M a, M b, M c)> triplets,
                                               Action<M, M> assertAreEqual) where M : ISemigroup<M> {
         foreach ((M, M, M ) triplet in triplets)
            Associativity(triplet, assertAreEqual);
      }


      // // partial function application
      // public static Action<IEnumerable<(M a, M b, M c)>> AssociativityMulti<M>(Action<M, M> assertAreEqual) where M : ISemigroup<M>
      //    => triplets => AssociativityMulti<M>(triplets, assertAreEqual);
   }
}
