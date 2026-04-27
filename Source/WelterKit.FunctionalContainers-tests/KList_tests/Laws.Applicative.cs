using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.KList_tests;

[TestClass]
public class Laws_Applicative {

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
      var funcs1 = ( u: new KList<Func<string, string>>( [ static x => x + "$"      ] ),
                     v: new KList<Func<int, string>   >( [ static x => x.ToString() ] ) );

      var funcs2 = ( u: new KList<Func<int, string>>( [ static x => x + "int" ] ),
                     v: new KList<Func<float, int> >( [ static x => (int)x    ] ) );

      var funcs3 = ( u: new KList<Func<int, TimeSpan>>( [ static x => TimeSpan.FromMinutes(x) ] ),
                     v: new KList<Func<string, int>  >( [ static x => x.Length                ] ) );

      var funcs4 = ( u: new KList<Func<(bool isNull, string s), int>    >( [ static x => x.isNull ? -1 : x.s.Length                     ] ),
                     v: new KList<Func<string?, (bool isNull, string s)>>( [ static x => x is null ? (true, string.Empty) : (false, x!) ] ) );


      var testValues1 = new[]
                           {
                              new KList<int>([]),
                              new KList<int>([1, 42, 999]),
                           };
      var testValues2 = new[]
                           {
                              new KList<float>([]),
                              new KList<float>([0.1f, 42.42f, 999.999f]),
                           };
      var testValues3 = new[]
                           {
                              new KList<string>([]),
                              new KList<string>([string.Empty, string.Empty]),
                              new KList<string>(["abc", "123", "XYZ"]),
                           };
      var testValues4 = new[]
                           {
                              new KList<string?>([]),
                              new KList<string?>([null]),
                              new KList<string?>([null, null, "abc XYZ"]),
                           };

      LawsTestHelpers.Multitest(funcs1, testValues1, testComposition);
      LawsTestHelpers.Multitest(funcs2, testValues2, testComposition);
      LawsTestHelpers.Multitest(funcs3, testValues3, testComposition);
      LawsTestHelpers.Multitest(funcs4, testValues4, testComposition);
   }


   private static void testIdentity<A>(KList<A> testList) {
      Laws.Applicative.Identity(testList,
                                collectionAssert_areEqual);
   }


   private static void testComposition<A, B, C>(K<KList, Func<B, C>> u,
                                                K<KList, Func<A, B>> v,
                                                K<KList, A> testList) {
      Laws.Applicative.Composition(u, v, testList, collectionAssert_areEqual);
   }


   private static void collectionAssert_areEqual<A>(K<KList, A> fa,
                                                    K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
