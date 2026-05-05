using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;

internal static class Helpers {
   internal static Writer<StringAccumulator, A> NewWriter<A>(ImmutableList<string> writerEntries, A value)
      => new(() => (new StringAccumulator(writerEntries), value));


   internal static void AssertWritersAreEqual<A>(K<Writer<StringAccumulator>, A> expected, K<Writer<StringAccumulator>, A> actual)
      => WriterAssert.AreEqual(expected, actual, StringAccumulatorAssert.AreEqual);
}
