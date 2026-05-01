using System;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal static partial class Laws {

   internal static class Monoid {
      // There exists an element e in S such that for every element a in S, the equalities e • a = a and a • e = a hold.
      public static void Identity<M>(M testValue, Action<M, M> assertAreEqual) where M : IMonoid<M> {
         assertAreEqual(testValue, M.Empty.Combine(testValue));
         assertAreEqual(testValue.Combine(M.Empty), testValue);
      }
   }
}
