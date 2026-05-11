using System;
using WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity() {
      ForReader<StringAccumulator>.TestIdentity(TestSet1.Reader_sum3, StringAccumulatorAssert.AreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      Laws.Applicative.Composition(TestSet1.ReaderFunc_int_string,
                                   TestSet1.ReaderFunc_decimal_int,
                                   TestSet1.Reader_sum3,
                                   AssertReadersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Homomorphism() {
      Laws.Applicative.Homomorphism<Reader<StringAccumulator>, string, int>("ABC 123", (string s) => s.Length, AssertReadersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Interchange() {
      Laws.Applicative.Interchange(TestSet1.ReaderFunc_int_string, 42, AssertReadersAreEqual);

      // TODO: more...
   }


   private static class ForReader<W> where W : IMonoid<W> {
      public static void TestIdentity<A>(Reader<W, A> testReader, Action<W, W> assertWriteesAreEqual)
         => Laws.Applicative.Identity(testReader, ReaderAssert.AreEqual<W, A>(assertWriteesAreEqual));


      public static void TestComposition<A, B, C>(Reader<W, Func<B, C>> u,
                                                  Reader<W, Func<A, B>> v,
                                                  Reader<W, A> w,
                                                  Action<K<Reader<W>, C>, K<Reader<W>, C>> assertWriteesAreEqual)
         => Laws.Applicative.Composition(u, v, w, assertWriteesAreEqual);
   }



}
