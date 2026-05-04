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
      testIdentity_string(TestSet1.Writer_sum3);

      // TODO: more...
   }


   [TestMethod]
   public void Composition() {
      Laws.Applicative.Composition(TestSet1.Func_int_string, 
                                   TestSet1.Func_decimal_int, 
                                   TestSet1.Writer_sum3, assertAreEqual);

      // TODO: more... ?
   }


   private static void testIdentity_string(Writer<StringAccumulator, decimal> testWriter) 
      => ForWriter<StringAccumulator>.TestIdentity(testWriter, StringAccumulatorAssert.AreEqual);


   // private static void testComposition_string<A, B, C>(( Writer<StringAccumulator, Func<B, C>> u,
   //                                                       Writer<StringAccumulator, Func<A, B>> v ) funcs,
   //                                                     Writer<StringAccumulator, A> w)
   //    => Laws.Applicative.Composition(funcs.u, funcs.v, w, assertAreEqual);


   private static void assertAreEqual<A>(K<Writer<StringAccumulator>, A> expected, K<Writer<StringAccumulator>, A> actual)
       => assertAreEqual(expected.As(), actual.As());


   private static void assertAreEqual<A>(Writer<StringAccumulator, A> expected, Writer<StringAccumulator, A> actual) {
      (StringAccumulator writer, A value) expectedRun = expected.RunWriter();
      (StringAccumulator writer, A value) actualRun = actual.RunWriter();
      Assert.AreEqual(expectedRun.value, actualRun.value, "value");
      CollectionAssert.AreEqual(expectedRun.writer.Entries, actualRun.writer.Entries, "writer (Entries)");
   }


   //    [TestMethod]
//    public void Homomorphism() {
//       static string func1(int x) => x.ToString();
//
//       int[] testValues1    = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//       int[] initialWriters1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//
//       multitestHomomorphism(func1, testValues1, initialWriters1);
//
//       // TODO: more...
//    }
//
//
//    [TestMethod]
//    public void Interchange() {
//       var state1 = new Writer<int, Func<int, string>>(s => (s, x => x.ToString()));
//
//       int[] testValues1    = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//       int[] initialWriters1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//
//       multitestInterchange(state1, testValues1, initialWriters1);
//
//       // TODO: more...
//    }


//    private static void multitestComposition<S, A, B, C>(( Writer<S, Func<B, C>> u,
//                                                              Writer<S, Func<A, B>> v ) funcs,
//                                                         Writer<S, A>[] testWriters,
//                                                         S[] sampleInitialWriters) {
//       foreach (Writer<S, A> state in testWriters)
//       foreach (S initialWriter in sampleInitialWriters) 
//          testComposition(funcs.u, funcs.v, state, initialWriter);
//    }


//    private static void testComposition<W,A, B, C>(Writer<W, Func<B, C>> u,
//                                                   Writer<W, Func<A, B>> v,
//                                                   Writer<W, A> testWriter,
//                                                   W sampleInitialWriter)
//       => Laws.Applicative.Composition(u,v,w)


//    private static void testHomomorphism<S, A, B>(Func<A, B> testFunc, A testWriter, S sampleInitialWriter)
//       => Laws.Applicative.Homomorphism(testWriter, testFunc, LawsTestHelpers.AreEqualFromWriter<S, B>(sampleInitialWriter));
//
//
//    private static void testInterchange<S, A, B>(Writer<S, Func<A, B>> u, A y, S sampleInitialWriter)
//       => Laws.Applicative.Interchange(u, y, LawsTestHelpers.AreEqualFromWriter<S, B>(sampleInitialWriter));
//
//
//    private static void multitestIdentity<S, A>(Writer<S, A>[] testWriters, S[] sampleInitialWriters) {
//       var combos = from testWriter in testWriters
//                    from initialWriter in sampleInitialWriters
//                    select (testWriter, initialWriter);
//       foreach ((Writer<S, A> testWriter, S initialWriter) in combos) {
//          testIdentity(testWriter, initialWriter);
//       }
//    }


//    private static void multitestHomomorphism<S,A, B>(Func<A, B> func, A[] values, S[] sampleInitialWriters) {
//       foreach (A value in values)
//       foreach (S initialWriter in sampleInitialWriters)
//          testHomomorphism(func, value, initialWriter);
//    }
//
//
//    private static void multitestInterchange<S, A, B>(Writer<S, Func<A, B>> func, A[] values, S[] sampleInitialWriters) {
//       foreach (A value in values)
//       foreach (S initialWriter in sampleInitialWriters) {
//          testInterchange(func, value, initialWriter);
//       }
//    }


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
