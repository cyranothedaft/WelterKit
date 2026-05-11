using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers_tests.ContainerTests.Reader_tests;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests;

internal static class Helpers {
   internal static Reader<StringAccumulator, A> NewReader<A>(ImmutableList<string> writerEntries, A value)
      => new(() => (new StringAccumulator(writerEntries), value));


   internal static void AssertReadersAreEqual<A>(K<Reader<StringAccumulator>, A> expected, K<Reader<StringAccumulator>, A> actual)
      => ReaderAssert.AreEqual(expected, actual, StringAccumulatorAssert.AreEqual);
}
