using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;

[TestClass]
public class Laws_Monad {
   [TestMethod]
   public void LeftIdentity() {
      Laws.Monad.LeftIdentity(42, x => newWriter(["ABC"], x.ToString()), assertAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void RightIdentity() {
      Laws.Monad.RightIdentity(newWriter(["XYZ"], 42), assertAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Associativity() {
      Laws.Monad.Associativity(newWriter(["DEF"], 42),
                               g: (int n) => newWriter(["LMNO"],  n.ToString()),
                               h: (string s) => newWriter(["PQ"], s + "$$"),
                               assertAreEqual);

      // TODO: more...
   }


   private Writer<StringAccumulator, A> newWriter<A>(ImmutableList<string> writerEntries, A value)
      => new(() => (new StringAccumulator(writerEntries), value));


   private void assertAreEqual<A>(K<Writer<StringAccumulator>, A> expected, K<Writer<StringAccumulator>, A> actual)
      => WriterAssert.AreEqual(expected, actual, StringAccumulatorAssert.AreEqual);

}
