using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity_Simple_Some() {
      Laws.Applicative.Identity(new Some<int>(42), Assert.AreEqual);
   }


   [TestMethod]
   public void Identity_Simple_None() {
      Laws.Applicative.Identity(new None<int>(), Assert.AreEqual);
   }


   [TestMethod]
   public void Composition_Simple_Some() {
      Laws.Applicative.Composition(Maybe.Pure<Func<string, string>>(static (string x) => x + "$"),
                                   Maybe.Pure<Func<int,    string>>(static (int x) => x.ToString()),
                                   new Some<int>(42),
                                   Assert.AreEqual);
   }


   [TestMethod]
   public void Composition_Simple_None() {
      Laws.Applicative.Composition(Maybe.Pure<Func<string, string>>(static (string x) => x + "$"),
                                   Maybe.Pure<Func<int,    string>>(static (int x) => x.ToString()),
                                   new None<int>(),
                                   Assert.AreEqual);
   }

}
