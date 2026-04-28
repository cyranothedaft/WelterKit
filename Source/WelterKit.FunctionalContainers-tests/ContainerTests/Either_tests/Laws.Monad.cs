using System;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public class Laws_Monad {

   [TestMethod]
   public void LeftIdentity() {
      static Either<string, string> func1(int a)     => a > 0                                            ? new Right<string, string>($"positive:{a}") : new Left<string, string>($"{a} is not positive");
      static Either<string, int>    func2(decimal a) => decimal.IsInteger(a)                             ? new Right<string, int>   ((int)a)          : new Left<string, int>   ($"{a} is not an integer");
      static Either<string, int>    func3(string a)  => int.TryParse(a, out int i)                       ? new Right<string, int>   (i)               : new Left<string, int>   ($"\"{a}\" is not int-parsable");
      static Either<string, float>  func4(string? a) => a is not null && float.TryParse(a!, out float f) ? new Right<string, float> (f)               : new Left<string, float> ("null or not-float-parsable");

      var testValues1 = new int[] { -42, -1, 0, 1, 42, 999 };
      var testValues2 = new decimal[] { -999.999M, -42M, -1M, -0.1M, decimal.Zero, 0.1M, 1M, 42M, 42.42M, 99.999M };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ", "42.86" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", "42.86", null };

      multitest(func1, testValues1);
      multitest(func2, testValues2);
      multitest(func3, testValues3);
      multitest(func4, testValues4);
      return;

      static void multitest<L, A, B>(Func<A, Either<L, B>> func, A[] testValues) {
         foreach (A testValue in testValues)
            testLeftIdentity(func, testValue);
      }
   }


   [TestMethod]
   public void RightIdentity() {
      var testValues1 = new Either<string, int>[]
                           {
                              new Left<string, int>("no value"),
                              new Right<string, int>(0),
                              new Right<string, int>(42),
                           };
      var testValues2 = new Either<string, float>[]
                           {
                             new Left<string, float>("no value"),
                             new Right<string, float>(0),
                             new Right<string, float>(42.42f),
                           };
      var testValues3 = new Either<string, string>[]
                           {
                             new Left<string, string>("no value"),
                             new Right<string, string>(string.Empty),
                             new Right<string, string>("abc XYZ"),
                           };
      var testValues4 = new Either<string, string?>[]
                           {
                             new Left<string, string?>("no value"),
                             new Right<string, string?>(null),
                             new Right<string, string?>("abc XYZ"),
                           };

      multitest(testValues1);
      multitest(testValues2);
      multitest(testValues3);
      multitest(testValues4);
      return;

      static void multitest<L, A>(Either<L, A>[] testValues) {
         foreach (Either<L, A> testValue in testValues)
            testRightIdentity(testValue);
      }
   }


   [TestMethod]
   public void Associativity() {
      var funcs1 = ( g: (Func<int,    Either<string, string>>)( static x => x % 2 == 0   ? new Right<string, string>($"even:{x}") : new Left<string, string>($"not even:{x}") ),
                     h: (Func<string, Either<string, string>>)( static x => x.Length > 0 ? new Right<string, string>($"some:{x}") : new Left<string, string>("empty string") ) );

      var funcs2 = ( g: (Func<string, Either<string, float>>)( static x => x.Length > 0 ? new Right<string, float>(x.Average(ch => (float)ch)) : new Left<string, float>("empty string")),
                     h: (Func<float,  Either<string, float>>)( static x => x > 0        ? new Right<string, float>(x * x)                      : new Left<string, float>($"{x} is not positive")));

      var testValues1 = new Either<string, int>[]
                           {
                              new Left<string, int>("no value"),
                              new Right<string, int>(0),
                              new Right<string, int>(41),
                              new Right<string, int>(42),
                           };
      var testValues2 = new Either<string, string>[]
                           {
                             new Left<string, string>("no value"),
                             new Right<string, string>(string.Empty),
                             new Right<string, string>("42 abc 42 xyz"),
                           };

      multitest(funcs1, testValues1);
      multitest(funcs2, testValues2);
      return;

      static void multitest<A, B>(( Func<A, Either<string, B>> g,
                                        Func<B, Either<string, B>> h ) funcs,
                                  Either<string, A>[] testValues) {
         foreach (Either<string, A> testValue in testValues)
            testAssociativity(funcs.g, funcs.h, testValue);
      }
   }


   private static void testLeftIdentity<L, A, B>(Func<A, Either<L, B>> testFunc, A testValue)
      => Laws.Monad.LeftIdentity(testValue, testFunc, Assert.AreEqual);


   private static void testRightIdentity<L, A>(Either<L, A> testList)
      => Laws.Monad.RightIdentity(testList, Assert.AreEqual);


   private static void testAssociativity<L, A, B>(Func<A, Either<L, B>> g,
                                                  Func<B, Either<L, B>> h,
                                                  Either<L, A> testValue)
      => Laws.Monad.Associativity(testValue, g, h, Assert.AreEqual);
}
