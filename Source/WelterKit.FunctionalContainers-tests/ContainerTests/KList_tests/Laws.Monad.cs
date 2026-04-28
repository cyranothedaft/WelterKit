using System;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.KList_tests;

[TestClass]
public class Laws_Monad {

   [TestMethod]
   public void LeftIdentity() {
      static KList<string> func1(int a) => new(Enumerable.Repeat(a.ToString(),       a).ToImmutableList());
      static KList<int> func2(float a) => new(Enumerable.Repeat((int)a,(int)MathF.Sqrt(a)).ToImmutableList());
      static KList<int> func3(string a) => new(a.ToCharArray().Select(ch => (int)ch).ToImmutableList());
      static KList<float> func4(string? a) => new(a is null ? [] : a!.ToCharArray().Select(ch => MathF.Sqrt((float)(int)ch)).ToImmutableList());

      var testValues1 = new int[] { 1, 42, 999 };
      var testValues2 = new float[] { 0.1f, 42.42f, 99.999f };
      var testValues3 = new string[] { string.Empty, "X", "42", "abc XYZ" };
      var testValues4 = new string?[] { string.Empty, "X", "42", "abc XYZ", null };

      multitestLeftIdentity(func1, testValues1);
      multitestLeftIdentity(func2, testValues2);
      multitestLeftIdentity(func3, testValues3);
      multitestLeftIdentity(func4, testValues4);
   }


   [TestMethod]
   public void RightIdentity() {
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

      multitestRightIdentity(testValues1);
      multitestRightIdentity(testValues2);
      multitestRightIdentity(testValues3);
      multitestRightIdentity(testValues4);
   }


   [TestMethod]
   public void Associativity() {
      var funcs1 = (g: (Func<int,    KList<string>>)(static x => new KList<string>(Enumerable.Repeat(x.ToString(), x).ToImmutableList())),
                    h: (Func<string, KList<string>>)(static x => new KList<string>(x.Select(ch => new string([ch]))  .ToImmutableList())));

      var funcs2 = (g: (Func<float, KList<int>>)(static x => new KList<int>( x.ToString("F").Where(char.IsDigit).Select(ch => int.Parse(ch+"")).ToImmutableList() )),
                    h: (Func<int,   KList<int>>)(static x => new KList<int>( x.ToString("D").Where(char.IsDigit).Select(ch => int.Parse(ch+"")).ToImmutableList() )) );

      var testValues1 = new int[] {1, 42, 999 };
      var testValues2 = new float[] { 0.1f, 42.42f, 999.999f };

      multitestAssociativity(funcs1, new KList<int  >(testValues1.ToImmutableList()));
      multitestAssociativity(funcs1, new KList<int  >([]));
      multitestAssociativity(funcs2, new KList<float>(testValues2.ToImmutableList()));
      multitestAssociativity(funcs2, new KList<float>([]));
   }


   private void multitestLeftIdentity<A,B>(Func<A, KList<B>> func, A[] testValues) {
      foreach (A testValue in testValues)
         testLeftIdentity(func, testValue);
   }


   private void multitestRightIdentity<A>(KList<A>[] testValues) {
      foreach (KList<A> testValue in testValues)
         testRightIdentity(testValue);
   }


   private void multitestAssociativity<A,B>(( Func<A, KList<B>> g, 
                                              Func<B, KList<B>> h ) funcs,
                                            KList<A> testValue) {
      // TODO: actual multi-test with multiple testValues?
      testAssociativity(funcs.g, funcs.h, testValue);
   }


   private static void testLeftIdentity<A, B>(Func<A, KList<B>> testFunc, A testValue)
      => Laws.Monad.LeftIdentity(testValue, testFunc, collectionAssert_areEqual);


   private static void testRightIdentity<A>(KList<A> testList)
      => Laws.Monad.RightIdentity(testList, collectionAssert_areEqual);


   private static void testAssociativity<A, B>(Func<A, KList<B>> g,
                                               Func<B, KList<B>> h,
                                               KList<A> testList)
      => Laws.Monad.Associativity(testList, g, h, collectionAssert_areEqual);


   private static void collectionAssert_areEqual<A>(K<KList, A> fa,
                                                    K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
