using System;
using WelterKit.FunctionalContainers_tests.Theory;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_tests;

[TestClass]
public class Laws_Monad {
   [TestMethod]
   public void LeftIdentity() {
      Laws.Monad.LeftIdentity(42, x => NewReader(["ABC"], x.ToString()), AssertReadersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void RightIdentity() {
      Laws.Monad.RightIdentity(NewReader(["XYZ"], 42), AssertReadersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Associativity() {
      Laws.Monad.Associativity(NewReader(["DEF"], 42),
                               g: (int n)    => NewReader(["LMNO"],  n.ToString()),
                               h: (string s) => NewReader(["PQ"], s + "$$"),
                               AssertReadersAreEqual);

      // TODO: more...
   }
}
