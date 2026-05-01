using System;
using WelterKit.FunctionalContainers_tests.Containers;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests.MatrixMult_Tests;

[TestClass]
public class Laws_Semigroup {
   [TestMethod]
   public void Associativity() {

      int[,][] matrices1 = [
         
         ];

      // TODO: more...
   }


   private static void testAssociativity<A>((MatrixMult<A> a, MatrixMult<A> b, MatrixMult<A> c) testMatrices)
      => Laws.Semigroup.Associativity(testMatrices.a,
                                      testMatrices.b,
                                      testMatrices.c,
                                      LawsTestHelpers.AreMatricesEqual);
}
