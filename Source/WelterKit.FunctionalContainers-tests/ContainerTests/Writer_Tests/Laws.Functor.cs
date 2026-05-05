using System;
using WelterKit.FunctionalContainers_tests.Theory;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity() {
      Laws.Functor.Identity(NewWriter(["ABC"], 42), AssertWritersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      Laws.Functor.Composition(NewWriter(["XYZ"], 42),
                               g: float.Parse,
                               h: (int n) => n.ToString(),
                               AssertWritersAreEqual);
      
      // TODO: more...
   }
}
