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
      var funcs1 = ( u: new Some<Func<string, string>>( static x => x + "$"      ),
                     v: new Some<Func<int, string>   >( static x => x.ToString() ) );

      var funcs2 = ( u: new Some<Func<int, string>>( static x => x + "int" ),
                     v: new Some<Func<float, int> >( static x => (int)x    ) );

      var funcs3 = ( u: new Some<Func<int, TimeSpan>>( static x => TimeSpan.FromMinutes(x) ),
                     v: new Some<Func<string, int>  >( static x => x.Length                ) );

      var funcs4 = ( u: new Some<Func<(bool isNull, string s), int>    >( static x => x.isNull ? -1 : x.s.Length                     ),
                     v: new Some<Func<string?, (bool isNull, string s)>>( static x => x is null ? (true, string.Empty) : (false, x!) ) );

      var testValues1 = new Maybe<int>[]
                           {
                              new None<int>(),
                              new Some<int>(0),
                              new Some<int>(42),
                           };
      var testValues2 = new Maybe<float>[]
                           {
                             new None<float>(),
                             new Some<float>(0),
                             new Some<float>(42.42f),
                           };
      var testValues3 = new Maybe<string>[]
                           {
                             new None<string>(),
                             new Some<string>(string.Empty),
                             new Some<string>("abc XYZ"),
                           };
      var testValues4 = new Maybe<string?>[]
                           {
                             new None<string?>(),
                             new Some<string?>(null),
                             new Some<string?>("abc XYZ"),
                           };

      LawsTestHelpers.Multitest(funcs1, testValues1, testComposition);
      LawsTestHelpers.Multitest(funcs2, testValues2, testComposition);
      LawsTestHelpers.Multitest(funcs3, testValues3, testComposition);
      LawsTestHelpers.Multitest(funcs4, testValues4, testComposition);
   }


   private static void testIdentity<A>(Maybe<A> testValue) {
      Laws.Applicative.Identity(testValue, Assert.AreEqual);
   }


   private static void testComposition<A, B, C>(Maybe<Func<B, C>> u,
                                                Maybe<Func<A, B>> v,
                                                Maybe<A> testValue) {
      Laws.Applicative.Composition(u, v, testValue, Assert.AreEqual);
   }


   private static void testHomomorphism<A, B>(Func<A, B> testFunc, A testValue) {
      Laws.Applicative.Homomorphism<Maybe, A, B>(testValue,
                                                 testFunc,
                                                 Assert.AreEqual);
   }

}
