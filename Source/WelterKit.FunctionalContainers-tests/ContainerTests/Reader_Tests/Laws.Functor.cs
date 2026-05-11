using System;
using WelterKit.FunctionalContainers_tests.Theory;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity() {
      Laws.Functor.Identity(NewReader(["ABC"], 42), AssertReadersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      Laws.Functor.Composition(NewReader(["XYZ"], 42),
                               g: float.Parse,
                               h: (int n) => n.ToString(),
                               AssertReadersAreEqual);
      
      // TODO: more...
   }
}
