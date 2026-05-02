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
      // square matrices

      Matrix<float> m2sq_0 = new(new float[,] { { 0, 0 },
                                                { 0, 0 } });
      Matrix<float> m2sq_42 = new(new float[,] { { 42, 42 },
                                                 { 42, 42 } });
      Matrix<float> m2sq_id = new(new float[,] { { 1, 0 },
                                                 { 0, 1 } });

      MatrixMult<float>[] matrices1 = new[] { m2sq_0, m2sq_42, m2sq_id }
                                          .Select(m => new MatrixMult<float>(m))
                                          .ToArray();

      multitestAssociativity(allTripletCombos(matrices1));

      // nonsquare matrices

      Matrix<int> a = new(new int[2, 3] { { 1, 1, 1 },
                                          { 1, 1, 1 } });
      Matrix<int> b = new(new int[3, 4] { { 1, 1, 1, 1 },
                                          { 1, 1, 1, 1 },
                                          { 1, 1, 1, 1 } });
      Matrix<int> c = new(new int[4, 2] { { 1, 1 },
                                          { 1, 1 },
                                          { 1, 1 },
                                          { 1, 1 } });

      testAssociativity((new MatrixMult<int>(a),
                         new MatrixMult<int>(b),
                         new MatrixMult<int>(c)));

      // TODO: more...
   }


   private IEnumerable<(T, T, T)> allTripletCombos<T>(ICollection<T> source)
         // TODO: improve/optimize?
      => from a in source
         from b in source
         from c in source
         select (a, b, c);


   private void testAssociativity<N>((MatrixMult<N> a, MatrixMult<N> b, MatrixMult<N> c) triplet)
         where N : INumber<N>
      => Laws.Semigroup.Associativity(triplet, LawsTestHelpers.AreMatricesEqual);


   private void multitestAssociativity<N>(IEnumerable<(MatrixMult<N> a, MatrixMult<N> b, MatrixMult<N> c)> triplets)
         where N : INumber<N>
      => Laws.Semigroup.AssociativityMulti(triplets, LawsTestHelpers.AreMatricesEqual);


   //private static Action<IEnumerable<(MatrixMult<N> a, MatrixMult<N> b, MatrixMult<N> c)>> multitestAssociativityFunc<N>() where N : INumber<N>
   //   => Laws.Semigroup.AssociativityMulti<MatrixMult<N>>(LawsTestHelpers.AreMatricesEqual);
}
