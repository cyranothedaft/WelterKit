using System;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.Containers;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests.MatrixMult_Tests;


internal static class LawsTestHelpers {
   public static void AreMatricesEqual<N>(MatrixMultData<N> m1, MatrixMultData<N> m2) where N : INumber<N>
      => MatrixAssert.AreEqual(m1.Value,
                               m2.Value);
}
