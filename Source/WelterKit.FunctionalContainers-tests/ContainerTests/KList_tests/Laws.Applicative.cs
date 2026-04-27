using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.KList_tests;

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


   [TestMethod]
   public void Homomorphism() {
      var func1 = (Func<int, string>            )( static x => x.ToString()                                   );
      var func2 = (Func<float, int>             )( static x => (int)x                                         );
      var func3 = (Func<string, int>            )( static x => x.Length                                       );
      var func4 = (Func<string?, (bool, string)>)( static x => x is null ? (true, string.Empty) : (false, x!) );

      var testValues1 = new int[] { 1, 42, 999 };
      var testValues2 = new float[] { 0.1f, 42.42f, 999.999f };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", null };

      multitestHomomorphism(func1, testValues1);
      multitestHomomorphism(func2, testValues2);
      multitestHomomorphism(func3, testValues3);
      multitestHomomorphism(func4, testValues4);
   }


   [TestMethod]
   public void Interchange() {
      static string         func1a(int x)     => x.ToString()                                  ;
      static string         func1b(int x)     => new string('*', x)                            ;
      static string         func1c(int x)     => "constant string"                             ;
      static int            func2a(float x)   => (int)x                                        ;
      static int            func2b(float x)   => -(int)MathF.Ceiling(x)                        ;
      static int            func2c(float x)   => 0                                             ;
      static int            func3a(string x)  => x.Length                                      ;
      static int            func3b(string x)  => x.Length == 0 ? -1 : (int)(char)x[0]          ;
      static int            func3c(string x)  => 42042                                         ;
      static (bool, string) func4a(string? x) => x is null ? (true, string.Empty) : (false, x!);
      static (bool, string) func4b(string? x) {
         var y = x + x + x ?? "qwerty";
         return (y.Length % 2 == 0, y);
      }
      static (bool, string) func4c(string? x) => (true, "constant string")                     ;

      var testValues1 = new int[] { 1, 42, 999 };
      var testValues2 = new float[] { 0.1f, 42.42f, 999.999f };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", null };

      // TODO: test more combinations of function lists

      multitestInterchange<int, string>            ([func1a, func1b, func1c], testValues1);
      multitestInterchange<int, string>            ([]                      , testValues1);
      multitestInterchange<float, int>             ([func2a, func2b, func2c], testValues2);
      multitestInterchange<float, int>             ([]                      , testValues2);
      multitestInterchange<string, int>            ([func3a, func3b, func3c], testValues3);
      multitestInterchange<string, int>            ([]                      , testValues3);
      multitestInterchange<string?, (bool, string)>([func4a, func4b, func4c], testValues4);
      multitestInterchange<string?, (bool, string)>([]                      , testValues4);
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


   private static void testHomomorphism<A, B>(A x, Func<A, B> f) {
      Laws.Applicative.Homomorphism<KList, A, B>(x, f, collectionAssert_areEqual);
   }


   private static void testInterchange<A, B>(KList<Func<A, B>> u, A y) {
      Laws.Applicative.Interchange(u, y, collectionAssert_areEqual);
   }


   private static void multitestHomomorphism<A, B>(Func<A, B> func, A[] values) {
      foreach (A value in values)
         testHomomorphism(value, func);
   }


   private static void multitestInterchange<A, B>(ImmutableList<Func<A, B>> funcList, A[] values) {
      KList<Func<A, B>> klist = new(funcList);
      foreach (A value in values)
         testInterchange(klist, value);
   }


   private static void collectionAssert_areEqual<A>(K<KList, A> fa,
                                                    K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
