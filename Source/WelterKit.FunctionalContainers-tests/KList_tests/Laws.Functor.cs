using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.KList_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity() {
      testIdentity(new KList<int>([]));
      testIdentity(new KList<int>([1, 42, 999]));
      testIdentity(new KList<float>([]));
      testIdentity(new KList<float>([0.1f, 42.42f, 999.999f]));
      testIdentity(new KList<string>([]));
      testIdentity(new KList<string>([string.Empty, string.Empty]));
      testIdentity(new KList<string>(["abc", "123", "XYZ"]));
      testIdentity(new KList<string?>([]));
      testIdentity(new KList<string?>([null]));
      testIdentity(new KList<string?>([null, null, "abc XYZ"]));
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

      testComposition(funcs1, new KList<int>([]));
      testComposition(funcs1, new KList<int>([1, 42, 999]));
      testComposition(funcs2, new KList<float>([]));
      testComposition(funcs2, new KList<float>([0.1f, 42.42f, 999.999f]));
      testComposition(funcs3, new KList<string>([]));
      testComposition(funcs3, new KList<string>([string.Empty, string.Empty]));
      testComposition(funcs3, new KList<string>(["abc", "123", "XYZ"]));
      testComposition(funcs4, new KList<string?>([]));
      testComposition(funcs4, new KList<string?>([null]));
      testComposition(funcs4, new KList<string?>(["", null, "abc XYZ"]));
   }


   private static void testIdentity<A>(KList<A> testList) {
      Laws.Functor.Identity(testList,
                            collectionAssert_areEqual);
   }


   private static void testComposition<A, B, C>(( Func<B, C> g,
                                                  Func<A, B> h ) funcs,
                                                KList<A> testList) {
      Laws.Functor.Composition(testList,
                               funcs.g,
                               funcs.h,
                               collectionAssert_areEqual);
   }


   private static void collectionAssert_areEqual<A>(K<KList, A> fa,
                                                    K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
