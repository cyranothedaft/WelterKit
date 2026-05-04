using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;

internal record StringAccumulator(ImmutableList<string> Entries) : IMonoid<StringAccumulator> {
   public StringAccumulator Combine(StringAccumulator rhs) => new(Entries.AddRange(rhs.Entries));
   public static StringAccumulator Empty { get; } = new([]);

   public override string ToString()
      => string.Join("\r\n", Entries);
}



internal static class StringAccumulatorAssert {
   internal static void AreEqual(StringAccumulator expected, StringAccumulator actual)
      => CollectionAssert.AreEqual(expected.Entries, actual.Entries);

   internal static void AreEqual(StringAccumulator expected, StringAccumulator actual, string msg)
      => CollectionAssert.AreEqual(expected.Entries, actual.Entries, msg);
}
