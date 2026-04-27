using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;


[TestClass]
public class Laws_Applicative {

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
      var funcs1 = ( u: new Right<string, Func<string, string>>( static x => x + "$"      ),
                     v: new Right<string, Func<int, string>   >( static x => x.ToString() ) );

      var funcs2 = ( u: new Right<string, Func<int, string>>( static x => x + "int" ),
                     v: new Right<string, Func<float, int> >( static x => (int)x    ) );

      var funcs3 = ( u: new Right<string, Func<int, TimeSpan>>( static x => TimeSpan.FromMinutes(x) ),
                     v: new Right<string, Func<string, int>  >( static x => x.Length                ) );

      var funcs4 = ( u: new Right<string, Func<(bool isNull, string s), int>    >( static x => x.isNull ? -1 : x.s.Length                     ),
                     v: new Right<string, Func<string?, (bool isNull, string s)>>( static x => x is null ? (true, string.Empty) : (false, x!) ) );

      var testValues1 = new Either<string, int>[]
                           {
                              new Left <string, int>("left value"),
                              new Right<string, int>(0),
                              new Right<string, int>(42),
                           };
      var testValues2 = new Either<string, float>[]
                           {
                             new Left <string, float>("left value"),
                             new Right<string, float>(0),
                             new Right<string, float>(42.42f),
                           };
      var testValues3 = new Either<string, string>[]
                           {
                             new Left <string, string>("left value"),
                             new Right<string, string>(string.Empty),
                             new Right<string, string>("abc XYZ"),
                           };
      var testValues4 = new Either<string, string?>[]
                           {
                             new Left <string, string?>("left value"),
                             new Right<string, string?>(null),
                             new Right<string, string?>("abc XYZ"),
                           };

      LawsTestHelpers.Multitest(funcs1, testValues1, testComposition, "left value");
      LawsTestHelpers.Multitest(funcs2, testValues2, testComposition, "left value");
      LawsTestHelpers.Multitest(funcs3, testValues3, testComposition, "left value");
      LawsTestHelpers.Multitest(funcs4, testValues4, testComposition, "left value");
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

      multitestHomomorphism<string, int, string>            (func1, testValues1);
      multitestHomomorphism<string, float, int>             (func2, testValues2);
      multitestHomomorphism<string, string, int>            (func3, testValues3);
      multitestHomomorphism<string, string?, (bool, string)>(func4, testValues4);
   }


   [TestMethod]
   public void Interchange() {
      static string         func1(int x)     => x.ToString()                                  ;
      static int            func2(float x)   => (int)x                                        ;
      static int            func3(string x)  => x.Length                                      ;
      static (bool, string) func4(string? x) => x is null ? (true, string.Empty) : (false, x!);

      var testValues1 = new int[] { 1, 42, 999 };
      var testValues2 = new float[] { 0.1f, 42.42f, 999.999f };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", null };

      multitestInterchange(func1, testValues1, "left value");
      multitestInterchange(func2, testValues2, "left value");
      multitestInterchange(func3, testValues3, "left value");
      multitestInterchange(func4, testValues4, "left value");
   }


   private static void testIdentity<A>(Either<string, A> testValue) {
      Laws.Applicative.Identity(testValue, Assert.AreEqual);
   }


   private static void testComposition<L, A, B, C>(Either<L, Func<B, C>> u,
                                                   Either<L, Func<A, B>> v,
                                                   Either<L, A> testValue)
      => Laws.Applicative.Composition(u, v, testValue, Assert.AreEqual);


   private static void testHomomorphism<L, A, B>(Func<A, B> testFunc, A testValue)
      => Laws.Applicative.Homomorphism<Either<L>, A, B>(testValue,
                                                        testFunc,
                                                        Assert.AreEqual);


   private static void testInterchange<L, A, B>(Either<L, Func<A, B>> u, A y)
      => Laws.Applicative.Interchange(u, y, Assert.AreEqual);


   private static void multitestHomomorphism<L, A, B>(Func<A, B> func, A[] values) {
      foreach (A value in values)
         testHomomorphism<L, A, B>(func, value);
   }


   private static void multitestInterchange<L, A, B>(Func<A, B> func, A[] values, L valueIfLeft) {
      foreach (A value in values) {
         testInterchange(new Right<L, Func<A, B>>(func       ), value);
         testInterchange(new Left <L, Func<A, B>>(valueIfLeft), value);
      }
   }
}
