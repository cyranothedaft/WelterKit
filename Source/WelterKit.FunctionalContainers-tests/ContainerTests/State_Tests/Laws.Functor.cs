using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity() {
      (int state, int value) runState1(int x) => (x, x);
      (int state, int value) runState2(int x) => (x+1, x-1);

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestIdentity(new State<int, int>(runState1), initialStates1);
      multitestIdentity(new State<int, int>(runState2), initialStates1);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      var funcs1 = ( g: (Func<string, string>)( static x => x + "$"   ),
                     h: (Func<int, string>   )( static x => x.ToString() ) );

      (int state, int value) runState1(int x) => (x, x);
      (int state, int value) runState2(int x) => (x+1, x-1);

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestComposition(funcs1, new State<int, int>(runState1), initialStates1);
      multitestComposition(funcs1, new State<int, int>(runState2), initialStates1);

      // TODO: more...
   }


   private static void testIdentity<S, A>(State<S, A> testState, S sampleInitialState)
      => Laws.Functor.Identity(testState,
                               (expected, actual) => LawsTestHelpers.AssertStatesAreEqual(expected, actual, sampleInitialState));


   private static void testComposition<S, A, B, C>(( Func<B, C> g,
                                                     Func<A, B> h ) funcs,
                                                   State<S, A> testState,
                                                   S sampleInitialState)
      => Laws.Functor.Composition(testState,
                                  funcs.g,
                                  funcs.h,
                                  (expected, actual) => LawsTestHelpers.AssertStatesAreEqual(expected, actual, sampleInitialState));


   private static void multitestIdentity<S, A>(State<S, A> testState, S[] sampleInitialStates) {
      foreach (S initialState in sampleInitialStates)
         testIdentity(testState, initialState);
   }


   private static void multitestComposition<S, A, B, C>(( Func<B, C> g,
                                                          Func<A, B> h ) funcs,
                                                        State<S, A> testState,
                                                        S[] sampleInitialStates) {
      foreach (S initialState in sampleInitialStates)
         testComposition(funcs, testState, initialState);
   }
}
