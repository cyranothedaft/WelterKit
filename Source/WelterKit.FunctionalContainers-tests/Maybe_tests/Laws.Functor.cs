using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;

[TestClass]
public class Laws_Functor {

   private static T id<T>(T x) => x;


   // Functors must preserve identity
   [TestMethod]
   public void Identity_Simple_Some() {
      Maybe<int> maybe = new Some<int>(42);
      Assert.AreEqual(42, ((Some<int>)Maybe.FMap(maybe, id)).Value);
   }


   // Functors must preserve composition
   [TestMethod]
   public void Composition_Simple() {
      Func<string, string> f = x => x + "$";
      Func<int, string> g = x => x.ToString();

      Maybe<int> maybe = new Some<int>(42);

      Assert.AreEqual((Maybe<string>)Maybe.FMap(Maybe.FMap(maybe, g), f),
                      (Maybe<string>)Maybe.FMap(maybe, x => f(g(x))));

   }

}
