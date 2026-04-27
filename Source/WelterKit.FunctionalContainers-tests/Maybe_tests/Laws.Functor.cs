using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity_Simple_Some() {
      Laws.Functor.Identity(new Some<int>(42),
                            Assert.AreEqual);
   }


   [TestMethod]
   public void Identity_Simple_None() {
      Laws.Functor.Identity(new None<int>(),
                            Assert.AreEqual);
   }


   [TestMethod]
   public void Composition_Simple_Some() {
      Laws.Functor.Composition(new Some<int>(42),
                               (string x) => x + "$",
                               (int x) => x.ToString(),
                               Assert.AreEqual);
   }


   [TestMethod]
   public void Composition_Simple_None() {
      Laws.Functor.Composition(new None<int>(),
                               (string x) => x + "$",
                               (int x) => x.ToString(),
                               Assert.AreEqual);
   }

}
