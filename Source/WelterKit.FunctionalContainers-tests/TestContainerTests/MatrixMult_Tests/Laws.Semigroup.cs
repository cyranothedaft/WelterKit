using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.Containers;
using WelterKit.FunctionalContainers_tests.Theory;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests.MatrixMult_Tests;

[TestClass]
public class Laws_Semigroup {
   [TestMethod]
   public void Associativity() {
      Matrix<float> m2sq_0 = new(new float[,] { { 0, 0 },
                                                { 0, 0 } });
      Matrix<float> m2sq_42 = new(new float[,] { { 42, 42 },
                                                 { 42, 42 } });
      Matrix<float> m2sq_id = new(new float[,] { { 1, 0 },
                                                 { 0, 1 } });

      MatrixMultData<float>[] matrices1 = new[] { m2sq_0, m2sq_42, m2sq_id }
                                          .Select(m => new MatrixMultData<float>(m))
                                          .ToArray();

      multitestAssociativity(allTripletCombos(matrices1));

      // TODO: more...
   }


   private IEnumerable<(T, T, T)> allTripletCombos<T>(ICollection<T> source)
         // TODO: improve/optimize?
      => from a in source
         from b in source
         from c in source
         select (a, b, c);


   private void multitestAssociativity<N>(IEnumerable<(MatrixMultData<N> a, MatrixMultData<N> b, MatrixMultData<N> c)> triplets)
         where N : INumber<N> {
      foreach (var triplet in triplets)
         testAssociativity(triplet);
   }


   private static void testAssociativity<N>((MatrixMultData<N> a, MatrixMultData<N> b, MatrixMultData<N> c) testMatrices)
         where N : INumber<N>
      => Laws.Semigroup.Associativity(testMatrices.a,
                                      testMatrices.b,
                                      testMatrices.c,
                                      LawsTestHelpers.AreMatricesEqual);
}
