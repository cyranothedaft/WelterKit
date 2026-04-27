using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity() {
      testIdentity(new None<int>());
      testIdentity(new Some<int>(0));
      testIdentity(new Some<int>(42));
      testIdentity(new None<float>());
      testIdentity(new Some<float>(0));
      testIdentity(new Some<float>(42.42f));
      testIdentity(new None<string>());
      testIdentity(new Some<string>(string.Empty));
      testIdentity(new Some<string>("abc XYZ"));
      testIdentity(new None<string?>());
      testIdentity(new Some<string?>(null));
      testIdentity(new Some<string?>("abc XYZ"));
   }


   [TestMethod]
   public void Composition() {
      var funcs1 = ( u: (Func<string, string>)( static x => x + "$"   ),
                     v: (Func<int, string>   )( static x => x.ToString() ) );

      var funcs2 = ( u: (Func<int, string>)( static x => x + "int" ),
                     v: (Func<float, int> )( static x => (int)x  ) );

      var funcs3 = ( u: (Func<int, TimeSpan>)( static x => TimeSpan.FromMinutes(x) ),
                     v: (Func<string, int>  )( static x => x.Length  ) );

      var funcs4 = ( u: (Func<(bool isNull, string s), int>    )( static x => x.isNull ? -1 : x.s.Length                     ),
                     v: (Func<string?, (bool isNull, string s)>)( static x => x is null ? (true, string.Empty) : (false, x!) ) );

      testComposition(funcs1, new None<int>());
      testComposition(funcs1, new Some<int>(0));
      testComposition(funcs1, new Some<int>(42));
      testComposition(funcs2, new None<float>());
      testComposition(funcs2, new Some<float>(0));
      testComposition(funcs2, new Some<float>(42.42f));
      testComposition(funcs3, new None<string>());
      testComposition(funcs3, new Some<string>(string.Empty));
      testComposition(funcs3, new Some<string>("abc XYZ"));
      testComposition(funcs4, new None<string?>());
      testComposition(funcs4, new Some<string?>(null));
      testComposition(funcs4, new Some<string?>("abc XYZ"));
   }


   // [TestMethod]
   // public void Homomorphism_Simple_None() {
   //    Laws.Applicative.Homomorphism(Maybe.Pure<Func<string, string>>(static (string x) => x + "$"),
   //                                  Maybe.Pure<Func<int,    string>>(static (int x) => x.ToString()),
   //                                  new None<int>(),
   //                                  Assert.AreEqual);
   // }


   private static void testIdentity<A>(Maybe<A> testValue) {
      Laws.Applicative.Identity(testValue, Assert.AreEqual);
   }


   private static void testComposition<A, B, C>(( Func<B, C> u,
                                                  Func<A, B> v ) funcs,
                                                Maybe<A> testValue) {
      Laws.Applicative.Composition(Maybe.Pure(funcs.u),
                                   Maybe.Pure(funcs.v),
                                   testValue,
                                   Assert.AreEqual);
   }

}
