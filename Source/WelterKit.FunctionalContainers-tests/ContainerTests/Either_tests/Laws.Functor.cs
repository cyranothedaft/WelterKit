using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity() {
      testIdentity(new Left <string, int>   ("no value"));
      testIdentity(new Right<string, int>   (0));
      testIdentity(new Right<string, int>   (42));
      testIdentity(new Left <string, float> ("no value"));
      testIdentity(new Right<string, float> (0));
      testIdentity(new Right<string, float> (42.42f));
      testIdentity(new Left <string, string>("no value"));
      testIdentity(new Right<string, string>(string.Empty));
      testIdentity(new Right<string, string>("abc XYZ"));
      testIdentity(new Left <string, string?>("no value"));
      testIdentity(new Right<string, string?>(null));
      testIdentity(new Right<string, string?>("abc XYZ"));
   }


   [TestMethod]
   public void Composition() {
      var funcs1 = ( g: (Func<string, string>)( static x => x + "$"   ),
                     h: (Func<int, string>   )( static x => x.ToString() ) );

      var funcs2 = ( g: (Func<int, string>)( static x => x + "int" ),
                     h: (Func<float, int> )( static x => (int)x  ) );

      var funcs3 = ( g: (Func<int, TimeSpan>)( static x => TimeSpan.FromMinutes(x) ),
                     h: (Func<string, int>  )( static x => x.Length  ) );

      var funcs4 = ( g: (Func<(bool isNull, string s), int>    )( static x => x.isNull ? -1 : x.s.Length                     ),
                     h: (Func<string?, (bool isNull, string s)>)( static x => x is null ? (true, string.Empty) : (false, x!) ) );

      testComposition(funcs1, new Left <string, int>   ("no value"));
      testComposition(funcs1, new Right<string, int>   (0));
      testComposition(funcs1, new Right<string, int>   (42));
      testComposition(funcs2, new Left <string, float> ("no value"));
      testComposition(funcs2, new Right<string, float> (0));
      testComposition(funcs2, new Right<string, float> (42.42f));
      testComposition(funcs3, new Left <string, string>("no value"));
      testComposition(funcs3, new Right<string, string>(string.Empty));
      testComposition(funcs3, new Right<string, string>("abc XYZ"));
      testComposition(funcs4, new Left <string, string?>("no value"));
      testComposition(funcs4, new Right<string, string?>(null));
      testComposition(funcs4, new Right<string, string?>("abc XYZ"));
   }


   private static void testIdentity<A>(Either<string, A> testValue)
      => Laws.Functor.Identity(testValue, Assert.AreEqual);


   private static void testComposition<L, A, B, C>(( Func<B, C> g,
                                                     Func<A, B> h ) funcs,
                                                   Either<L, A> testValue)
      => Laws.Functor.Composition(testValue,
                                  funcs.g,
                                  funcs.h,
                                  Assert.AreEqual);

}
