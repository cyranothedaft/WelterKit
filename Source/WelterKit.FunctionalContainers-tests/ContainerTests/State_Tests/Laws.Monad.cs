using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_tests;

[TestClass]
public class Laws_Monad {
   [TestMethod]
   public void LeftIdentity() {
      State<int, string> func1(string x) => new(s => (s, x + "$"));

      Func<string, State<int, string>>[] funcs1 = [func1];

      string[] testValues1 = [string.Empty, "X", "42", "abc XYZ"];

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestLeftIdentity(funcs1, testValues1, initialStates1);

      // TODO: more...
   }


   [TestMethod]
   public void RightIdentity() {
      State<int, int>[] testStates1 =
         [
            new(x => (x, x)),
            new(x => (x + 1, x - 1)),
            new(_ => (0, 0))
         ];

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestRightIdentity(testStates1, initialStates1);

      // TODO: more...
   }


   [TestMethod]
   public void Associativity() {
      ( Func<int,    State<int, string>>,
        Func<string, State<int, string>> ) funcs1 = ( g: n   => new State<int, string>(s => (s, n.ToString())),
                                                      h: str => new State<int, string>(s => (s, str + "$"))   );

      State<int, int>[] testStates1 =
         [
            new(x => (x, x)),
            new(x => (x + 1, x - 1)),
            new(_ => (0, 0))
         ];

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestAssociativity(funcs1, testStates1, initialStates1);

      // TODO: more...
   }


   private void multitestLeftIdentity<S, A, B>(Func<A, State<S, B>>[] funcs, A[] testValues, S[] sampleInitialStates) {
      foreach (Func<A, State<S, B>> func in funcs)
      foreach (A testValue in testValues)
      foreach (S initialState in sampleInitialStates)
         testLeftIdentity(func, testValue, initialState);
   }


   private void multitestRightIdentity<S, A>(State<S, A>[] testStates, S[] sampleInitialStates) {
      foreach (State<S, A> testState in testStates)
      foreach (S initialState in sampleInitialStates)
         testRightIdentity(testState, initialState);
   }


   private void multitestAssociativity<S, A, B>(( Func<A, State<S, B>> g,
                                                  Func<B, State<S, B>> h ) funcs,
                                                State<S, A>[] testStates,
                                                S[] sampleInitialStates) {
      foreach (State<S, A> testState in testStates)
      foreach (S initialState in sampleInitialStates)
         testAssociativity(funcs.g, funcs.h, testState, initialState);
   }


   private static void testLeftIdentity<S, A, B>(Func<A, State<S, B>> testFunc, A testValue, S sampleInitialState)
      => Laws.Monad.LeftIdentity(testValue, testFunc, LawsTestHelpers.AreEqualFromState<S, B>(sampleInitialState));


   private static void testRightIdentity<S, A>(State<S, A> testState, S sampleInitialState)
      => Laws.Monad.RightIdentity(testState, LawsTestHelpers.AreEqualFromState<S, A>(sampleInitialState));


   private static void testAssociativity<S, A, B>(Func<A, State<S, B>> g,
                                                  Func<B, State<S, B>> h,
                                                  State<S, A> testState, S sampleInitialState)
      => Laws.Monad.Associativity(testState, g, h, LawsTestHelpers.AreEqualFromState<S, B>(sampleInitialState));
}
