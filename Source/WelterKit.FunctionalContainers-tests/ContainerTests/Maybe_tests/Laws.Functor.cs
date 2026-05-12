using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Maybe_tests;

[TestClass]
public class Laws_Functor : Laws_Functor_Tests<Maybe> {
   protected override void AssertAreEqual<A>(K<Maybe, A> expected, K<Maybe, A> actual)
      => Assert.AreEqual(expected.As(), actual.As());


   protected override K<Maybe, int>[] GetTestSubjects_int(int x)
      =>
         [
            new None<int>(),
            new Some<int>(x),
         ];
   //
   //
   //
   //
   // public void Identity() {
   //    testIdentity(new None<int>());
   //    testIdentity(new Some<int>(0));
   //    testIdentity(new Some<int>(42));
   //    testIdentity(new None<float>());
   //    testIdentity(new Some<float>(0));
   //    testIdentity(new Some<float>(42.42f));
   //    testIdentity(new None<string>());
   //    testIdentity(new Some<string>(string.Empty));
   //    testIdentity(new Some<string>("abc XYZ"));
   //    testIdentity(new None<string?>());
   //    testIdentity(new Some<string?>(null));
   //    testIdentity(new Some<string?>("abc XYZ"));
   // }
   //
   //
   // [TestMethod]
   // public void Composition() {
   //    var funcs1 = ( g: (Func<string, string>)( static x => x + "$"   ),
   //                   h: (Func<int, string>   )( static x => x.ToString() ) );
   //
   //    var funcs2 = ( g: (Func<int, string>)( static x => x + "int" ),
   //                   h: (Func<float, int> )( static x => (int)x  ) );
   //
   //    var funcs3 = ( g: (Func<int, TimeSpan>)( static x => TimeSpan.FromMinutes(x) ),
   //                   h: (Func<string, int>  )( static x => x.Length  ) );
   //
   //    var funcs4 = ( g: (Func<(bool isNull, string s), int>    )( static x => x.isNull ? -1 : x.s.Length                     ),
   //                   h: (Func<string?, (bool isNull, string s)>)( static x => x is null ? (true, string.Empty) : (false, x!) ) );
   //
   //    testComposition(funcs1, new None<int>());
   //    testComposition(funcs1, new Some<int>(0));
   //    testComposition(funcs1, new Some<int>(42));
   //    testComposition(funcs2, new None<float>());
   //    testComposition(funcs2, new Some<float>(0));
   //    testComposition(funcs2, new Some<float>(42.42f));
   //    testComposition(funcs3, new None<string>());
   //    testComposition(funcs3, new Some<string>(string.Empty));
   //    testComposition(funcs3, new Some<string>("abc XYZ"));
   //    testComposition(funcs4, new None<string?>());
   //    testComposition(funcs4, new Some<string?>(null));
   //    testComposition(funcs4, new Some<string?>("abc XYZ"));
   // }
   //
   //
   // private static void testIdentity<A>(Maybe<A> testValue) {
   //    Laws.Functor.Identity(testValue, Assert.AreEqual);
   // }
   //
   //
   // private static void testComposition<A, B, C>(( Func<B, C> g,
   //                                                Func<A, B> h ) funcs,
   //                                              Maybe<A> testValue) {
   //    Laws.Functor.Composition(testValue,
   //                             funcs.g,
   //                             funcs.h,
   //                             Assert.AreEqual);
   // }

}
