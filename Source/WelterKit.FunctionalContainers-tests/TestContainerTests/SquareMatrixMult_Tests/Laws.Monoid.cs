using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.Containers;
using WelterKit.FunctionalContainers_tests.Theory;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests.SquareMatrixMult_Tests;

[TestClass]
public class Laws_Monoid {
   [TestMethod]
   public void Identity() {
      Matrix<float> m3sq_0 = new(new float[,] { { 0, 0, 0 },
                                                { 0, 0, 0 },
                                                { 0, 0, 0 } });
      Matrix<float> m3sq_42 = new(new float[,] { { 42, 42, 42 },
                                                 { 42, 42, 42 },
                                                 { 42, 42, 42 } });
      Matrix<float> m3sq_id = new(new float[,] { { 1, 0, 0 },
                                                 { 0, 1, 0 },
                                                 { 0, 0, 1 } });

      testIdentity(new SquareMatrixMultData_3<float>(m3sq_0));
      testIdentity(new SquareMatrixMultData_3<float>(m3sq_42));
      testIdentity(new SquareMatrixMultData_3<float>(m3sq_id));

      // TODO: more...
   }


   private static void testIdentity<N>(SquareMatrixMultData_3<N> testValue)
         where N : INumber<N>
      => Laws.Monoid.Identity(testValue, LawsTestHelpers.AreMatricesEqual);
}
