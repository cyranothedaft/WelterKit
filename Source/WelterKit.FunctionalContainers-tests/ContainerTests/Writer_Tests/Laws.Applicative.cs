using System;
using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity() {
      ForWriter<StringAccumulator>.TestIdentity(TestSet1.Writer_sum3, StringAccumulatorAssert.AreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      Laws.Applicative.Composition(TestSet1.WriterFunc_int_string,
                                   TestSet1.WriterFunc_decimal_int,
                                   TestSet1.Writer_sum3,
                                   assertWritersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Homomorphism() {
      Laws.Applicative.Homomorphism<Writer<StringAccumulator>, string, int>("ABC 123", s => s.Length, assertWritersAreEqual);

      // TODO: more...
   }


   [TestMethod]
   public void Interchange() {
      Laws.Applicative.Interchange(TestSet1.WriterFunc_int_string, 42, assertWritersAreEqual);

      // TODO: more...
   }


   private static void assertWritersAreEqual<A>(K<Writer<StringAccumulator>, A> expected, K<Writer<StringAccumulator>, A> actual)
      => assertWritersAreEqual(expected.As(), actual.As());


   private static void assertWritersAreEqual<A>(Writer<StringAccumulator, A> expected, Writer<StringAccumulator, A> actual) {
      (StringAccumulator writer, A value) expectedRun = expected.RunWriter();
      (StringAccumulator writer, A value) actualRun = actual.RunWriter();
      Assert.AreEqual(expectedRun.value, actualRun.value, "value");
      StringAccumulatorAssert.AreEqual(expectedRun.writer, actualRun.writer, "writer");
   }





   private static class ForWriter<W> where W : IMonoid<W> {
      public static void TestIdentity<A>(Writer<W, A> testWriter, Action<W, W> assertWriteesAreEqual)
         => Laws.Applicative.Identity(testWriter, WriterAssert.AreEqual<W, A>(assertWriteesAreEqual));


      public static void TestComposition<A, B, C>(Writer<W, Func<B, C>> u,
                                                  Writer<W, Func<A, B>> v,
                                                  Writer<W, A> w,
                                                  Action<K<Writer<W>, C>, K<Writer<W>, C>> assertWriteesAreEqual)
         => Laws.Applicative.Composition(u, v, w, assertWriteesAreEqual);
   }



}
