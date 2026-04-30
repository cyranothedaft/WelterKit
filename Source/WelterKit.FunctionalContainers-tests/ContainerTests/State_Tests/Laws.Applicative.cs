using System;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity() {
      State<int, int>[] testStates1 =
         [
            new(x => (x, x)),
            new(x => (x + 1, x - 1)),
            new(_ => (0, 0))
         ];

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestIdentity(testStates1, initialStates1);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      ( State<int, Func<string, string>> u,
        State<int, Func<int,    string>> v) funcs1 = ( u: new State<int, Func<string, string>>(s => (s, str => str + "$")),
                                                       v: new State<int, Func<int, string>>   (s => (s, n => n.ToString())) );

      State<int, int>[] testStates1 =
         [
            new(x => (x, x)),
            new(x => (x + 1, x - 1)),
            new(_ => (0, 0))
         ];

      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestComposition(funcs1, testStates1, initialStates1);

      // TODO: more... ?
   }


   [TestMethod]
   public void Homomorphism() {
      static string func1(int x) => x.ToString();

      int[] testValues1    = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestHomomorphism(func1, testValues1, initialStates1);

      // TODO: more...
   }


   [TestMethod]
   public void Interchange() {
      var state1 = new State<int, Func<int, string>>(s => (s, x => x.ToString()));

      int[] testValues1    = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
      int[] initialStates1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];

      multitestInterchange(state1, testValues1, initialStates1);

      // TODO: more...
   }


   private static void testIdentity<S, A>(State<S, A> testState, S sampleInitialState)
      => Laws.Applicative.Identity(testState, LawsTestHelpers.AreEqualFromState<S, A>(sampleInitialState));


   private static void testComposition<S,A, B, C>(State<S, Func<B, C>> u,
                                                  State<S, Func<A, B>> v,
                                                  State<S, A> testState,
                                                  S sampleInitialState)
      => Laws.Applicative.Composition(u, v, testState, LawsTestHelpers.AreEqualFromState<S, C>(sampleInitialState));


   private static void testHomomorphism<S, A, B>(Func<A, B> testFunc, A testState, S sampleInitialState)
      => Laws.Applicative.Homomorphism(testState, testFunc, LawsTestHelpers.AreEqualFromState<S, B>(sampleInitialState));


   private static void testInterchange<S, A, B>(State<S, Func<A, B>> u, A y, S sampleInitialState)
      => Laws.Applicative.Interchange(u, y, LawsTestHelpers.AreEqualFromState<S, B>(sampleInitialState));


   private static void multitestIdentity<S, A>(State<S, A>[] testStates, S[] sampleInitialStates) {
      var combos = from testState in testStates
                   from initialState in sampleInitialStates
                   select (testState, initialState);
      foreach ((State<S, A> testState, S initialState) in combos) {
         testIdentity(testState, initialState);
      }
   }


   private static void multitestComposition<S, A, B, C>(( State<S, Func<B, C>> u,
                                                              State<S, Func<A, B>> v ) funcs,
                                                        State<S, A>[] testStates,
                                                        S[] sampleInitialStates) {
      foreach (State<S, A> state in testStates)
      foreach (S initialState in sampleInitialStates) 
         testComposition(funcs.u, funcs.v, state, initialState);
   }


   private static void multitestHomomorphism<S,A, B>(Func<A, B> func, A[] values, S[] sampleInitialStates) {
      foreach (A value in values)
      foreach (S initialState in sampleInitialStates)
         testHomomorphism(func, value, initialState);
   }


   private static void multitestInterchange<S, A, B>(State<S, Func<A, B>> func, A[] values, S[] sampleInitialStates) {
      foreach (A value in values)
      foreach (S initialState in sampleInitialStates) {
         testInterchange(func, value, initialState);
      }
   }
}
