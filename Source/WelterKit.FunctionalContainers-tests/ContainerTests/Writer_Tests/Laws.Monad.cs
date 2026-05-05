using System;
using WelterKit.FunctionalContainers_tests.Theory;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;

[TestClass]
public class Laws_Monad {
   [TestMethod]
   public void LeftIdentity() {
      Laws.Monad.LeftIdentity(42, x => NewWriter(["ABC"], x.ToString()), AssertWritersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void RightIdentity() {
      Laws.Monad.RightIdentity(NewWriter(["XYZ"], 42), AssertWritersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Associativity() {
      Laws.Monad.Associativity(NewWriter(["DEF"], 42),
                               g: (int n)    => NewWriter(["LMNO"],  n.ToString()),
                               h: (string s) => NewWriter(["PQ"], s + "$$"),
                               AssertWritersAreEqual);

      // TODO: more...
   }
}
