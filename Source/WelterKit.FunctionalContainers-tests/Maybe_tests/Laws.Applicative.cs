using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity_Simple_Some() {
      Laws.Applicative.Identity(new Some<int>(42), Assert.AreEqual);
   }


   // Functors must preserve composition
   [TestMethod]
   public void Composition_Simple() {
      Laws.Applicative.Composition(new Some<int>(42),
                                   static (string x) => x + "$",
                                   static (int x) => x.ToString(),
                                   Assert.AreEqual);
   }

}
