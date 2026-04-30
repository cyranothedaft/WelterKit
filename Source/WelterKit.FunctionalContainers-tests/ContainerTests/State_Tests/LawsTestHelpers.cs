using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_tests;

internal static class LawsTestHelpers {
   // partial application of funcction AssertStatesAreEqual
   internal static Action<K<State<S>, A>, K<State<S>, A>> AreEqualFromState<S, A>(S sampleInitialState)
      => (expected, actual) => AssertStatesAreEqual(expected, actual, sampleInitialState);


   internal static void AssertStatesAreEqual<S, A>(K<State<S>, A> expected, K<State<S>, A> actual, S sampleInitialState)
      => AssertStatesAreEqual(expected.As(), actual.As(), sampleInitialState);


   internal static void AssertStatesAreEqual<S, A>(State<S, A> expected, State<S, A> actual, S sampleInitialState) {
      Assert.AreEqual(expected.runState(sampleInitialState),
                      actual  .runState(sampleInitialState));
   }
}
