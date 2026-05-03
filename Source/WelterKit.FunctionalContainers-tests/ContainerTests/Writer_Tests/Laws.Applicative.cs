using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;


[TestClass]
public class Laws_Applicative {

   private static class ForWriter<W> where W : IMonoid<W> {
      public static void TestIdentity<A>(Writer<W, A> testWriter, Action<W, W> assertWriteesAreEqual)
         => Laws.Applicative.Identity(testWriter, WriterAssert.AreEqual<W, A>(assertWriteesAreEqual));

   }



   private static class Test1 {
      public static Writer<StringAccumulator, decimal> TestWriter { get; }
         = getFinal(sum3);


      static Writer<StringAccumulator, decimal> getFinal(Func<decimal, decimal, decimal, decimal> combineResults)
         => Writer<StringAccumulator>.Pure(Fn.Curry(combineResults))
                                     .Apply(startWith(42.42M))
                                     .Apply(then(42.42M))
                                     .Apply(andFinally(42.42M))
                                     .As();

      static Writer<StringAccumulator, decimal> startWith(decimal value)
         => new(() => (new StringAccumulator([$"Starting with: {value}"]), value));

      static Writer<StringAccumulator, decimal> then(decimal value) {
         decimal h = value / 2;
         return new(() => (new StringAccumulator([$"Then, halved: {h}"]), h));
      }

      static Writer<StringAccumulator, decimal> andFinally(decimal value) {
         decimal t = value * 3;
         return new(() => (new StringAccumulator([$"And, finally, tripled: {t}"]), t));
      }

      static N sum3<N>(N a, N b, N c) where N : INumber<N> => a + b + c;
   }


   [TestMethod]
   public void Identity() {
      testIdentity_string(Test1.TestWriter);

      // TODO: more...
   }


   private static void testIdentity_string(Writer<StringAccumulator, decimal> testWriter) 
      => ForWriter<StringAccumulator>.TestIdentity(testWriter, StringAccumulatorAssert.AreEqual);


//    [TestMethod]
//    public void Composition() {
//       ( Writer<int, Func<string, string>> u,
//         Writer<int, Func<int,    string>> v) funcs1 = ( u: new Writer<int, Func<string, string>>(s => (s, str => str + "$")),
//                                                        v: new Writer<int, Func<int, string>>   (s => (s, n => n.ToString())) );
//
//       Writer<int, int>[] testWriters1 =
//          [
//             new(x => (x, x)),
//             new(x => (x + 1, x - 1)),
//             new(_ => (0, 0))
//          ];
//
//       int[] initialWriters1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//
//       multitestComposition(funcs1, testWriters1, initialWriters1);
//
//       // TODO: more... ?
//    }
//
//
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
//
//
//
//
//    private static void testComposition<S,A, B, C>(Writer<S, Func<B, C>> u,
//                                                   Writer<S, Func<A, B>> v,
//                                                   Writer<S, A> testWriter,
//                                                   S sampleInitialWriter)
//       => Laws.Applicative.Composition(u, v, testWriter, LawsTestHelpers.AreEqualFromWriter<S, C>(sampleInitialWriter));
//
//
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
//
//
//    private static void multitestComposition<S, A, B, C>(( Writer<S, Func<B, C>> u,
//                                                               Writer<S, Func<A, B>> v ) funcs,
//                                                         Writer<S, A>[] testWriters,
//                                                         S[] sampleInitialWriters) {
//       foreach (Writer<S, A> state in testWriters)
//       foreach (S initialWriter in sampleInitialWriters) 
//          testComposition(funcs.u, funcs.v, state, initialWriter);
//    }
//
//
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
}



internal static class WriterAssert {
   public static void AreEqual<W, A>(K<Writer<W>, A> expected,
                                     K<Writer<W>, A> actual,
                                     Action<W, W> assertAreEqual) where W : IMonoid<W>
      => AreEqual(expected.As(), actual.As(), assertAreEqual);


   public static void AreEqual<W, A>(Writer<W, A> expected,
                                     Writer<W, A> actual,
                                     Action<W, W> assertAreEqual) where W : IMonoid<W> {
      (W writer, A value) exp = expected.RunWriter();
      (W writer, A value) act = actual  .RunWriter();

      Assert.AreEqual(exp.value, act.value);
      assertAreEqual(exp.writer, act.writer);
   }

   // partial function application
   public static Action<K<Writer<W>, A>, K<Writer<W>, A>> AreEqual<W, A>(Action<W, W> assertWriteesAreEqual) where W : IMonoid<W>
      => (expected, actual) => AreEqual(expected, actual, assertWriteesAreEqual);
}
