using System;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_tests;

[TestClass]
public class Laws_Monad {
=====TODO=====
   [TestMethod]
   public void LeftIdentity() {
      static State<string> func1(int a)     => a > 0 ? new Some<string>($"positive:{a}") : new None<string>();
      static State<int>    func2(decimal a) => decimal.IsInteger(a) ? new Some<int>((int)a) : new None<int>();
      static State<int>    func3(string a)  => int.TryParse(a, out int i) ? new Some<int>(i) : new None<int>();
      static State<float>  func4(string? a) => a is not null && float.TryParse(a!, out float f) ? new Some<float>(f) : new None<float>();

      var testValues1 = new int[] { -42, -1, 0, 1, 42, 999 };
      var testValues2 = new decimal[] { -999.999M, -42M, -1M, -0.1M, decimal.Zero, 0.1M, 1M, 42M, 42.42M, 99.999M };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ", "42.86" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", "42.86", null };

      multitestLeftIdentity(func1, testValues1);
      multitestLeftIdentity(func2, testValues2);
      multitestLeftIdentity(func3, testValues3);
      multitestLeftIdentity(func4, testValues4);
   }


   [TestMethod]
   public void RightIdentity() {
      var testValues1 = new State<int>[]
                           {
                              new None<int>(),
                              new Some<int>(0),
                              new Some<int>(42),
                           };
      var testValues2 = new State<float>[]
                           {
                             new None<float>(),
                             new Some<float>(0),
                             new Some<float>(42.42f),
                           };
      var testValues3 = new State<string>[]
                           {
                             new None<string>(),
                             new Some<string>(string.Empty),
                             new Some<string>("abc XYZ"),
                           };
      var testValues4 = new State<string?>[]
                           {
                             new None<string?>(),
                             new Some<string?>(null),
                             new Some<string?>("abc XYZ"),
                           };

      multitestRightIdentity(testValues1);
      multitestRightIdentity(testValues2);
      multitestRightIdentity(testValues3);
      multitestRightIdentity(testValues4);
   }


   [TestMethod]
   public void Associativity() {
      var funcs1 = (g: (Func<int, State<string>>)(static x => x % 2       == 0 ? new Some<string>($"even:{x}") : new None<string>()),
                    h: (Func<string, State<string>>)(static x => x.Length > 0 ? new Some<string>($"some:{x}") : new None<string>()));

      var funcs2 = (g: (Func<string, State<float>>)(static x => x.Length > 0 ? new Some<float>(x.Average(ch => (float)(int)ch)) : new None<float>()),
                    h: (Func<float, State<float>>)(static x => x         > 0 ? new Some<float>(x * x) : new None<float>()));

      var testValues1 = new State<int>[]
                           {
                              new None<int>(),
                              new Some<int>(0),
                              new Some<int>(41),
                              new Some<int>(42),
                           };
      var testValues2 = new State<string>[]
                           {
                             new None<string>(),
                             new Some<string>(string.Empty),
                             new Some<string>("42 abc 42 xyz"),
                           };

      multitestAssociativity(funcs1, testValues1);
      multitestAssociativity(funcs2, testValues2);
   }


   private void multitestLeftIdentity<A,B>(Func<A, State<B>> func, A[] testValues) {
      foreach (A testValue in testValues)
         testLeftIdentity(func, testValue);
   }


   private void multitestRightIdentity<A>(State<A>[] testValues) {
      foreach (State<A> testValue in testValues)
         testRightIdentity(testValue);
   }


   private void multitestAssociativity<A, B>(( Func<A, State<B>> g,
                                               Func<B, State<B>> h ) funcs,
                                             State<A>[] testValues) {
      foreach (State<A> testValue in testValues)
         testAssociativity(funcs.g, funcs.h, testValue);
   }


   private static void testLeftIdentity<A, B>(Func<A, State<B>> testFunc, A testValue)
      => Laws.Monad.LeftIdentity(testValue, testFunc, Assert.AreEqual);


   private static void testRightIdentity<A>(State<A> testList)
      => Laws.Monad.RightIdentity(testList, Assert.AreEqual);


   private static void testAssociativity<A, B>(Func<A, State<B>> g,
                                               Func<B, State<B>> h,
                                               State<A> testValue)
      => Laws.Monad.Associativity(testValue, g, h, Assert.AreEqual);
}
