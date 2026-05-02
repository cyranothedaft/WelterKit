using System;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.Containers;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests;


internal static class LawsTestHelpers {
   public static void AreMatricesEqual<N>(MatrixMult<N> m1, MatrixMult<N> m2) where N : INumber<N>
      => MatrixAssert.AreEqual(m1.Value,
                               m2.Value);
}
