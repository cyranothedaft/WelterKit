// using System;
// using WelterKit.FunctionalContainers_tests.Theory;
// using WelterKit.FunctionalContainers.Containers;
// using WelterKit.FunctionalContainers.Framework;
//
//
//
// namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;
//
// [TestClass]
// public class Laws_Functor {
//
//    [TestMethod]
//    public void Identity() {
//       (int state, int value) runWriter1(int x) => (x, x);
//       (int state, int value) runWriter2(int x) => (x+1, x-1);
//
//       int[] initialWriters1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//
//       multitestIdentity(new Writer<int, int>(runWriter1), initialWriters1);
//       multitestIdentity(new Writer<int, int>(runWriter2), initialWriters1);
//
//       // TODO: more...
//    }
//
//
//    [TestMethod]
//    public void Composition() {
//       var funcs1 = ( g: (Func<string, string>)( static x => x + "$"   ),
//                      h: (Func<int, string>   )( static x => x.ToString() ) );
//
//       (int state, int value) runWriter1(int x) => (x, x);
//       (int state, int value) runWriter2(int x) => (x+1, x-1);
//
//       int[] initialWriters1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
//
//       multitestComposition(funcs1, new Writer<int, int>(runWriter1), initialWriters1);
//       multitestComposition(funcs1, new Writer<int, int>(runWriter2), initialWriters1);
//
//       // TODO: more...
//    }
//
//
//    private static void testIdentity<S, A>(Writer<S, A> testWriter, S sampleInitialWriter)
//       => Laws.Functor.Identity(testWriter,
//                                (expected, actual) => LawsTestHelpers.AssertWritersAreEqual(expected, actual, sampleInitialWriter));
//
//
//    private static void testComposition<S, A, B, C>(( Func<B, C> g,
//                                                      Func<A, B> h ) funcs,
//                                                    Writer<S, A> testWriter,
//                                                    S sampleInitialWriter)
//       => Laws.Functor.Composition(testWriter,
//                                   funcs.g,
//                                   funcs.h,
//                                   (expected, actual) => LawsTestHelpers.AssertWritersAreEqual(expected, actual, sampleInitialWriter));
//
//
//    private static void multitestIdentity<S, A>(Writer<S, A> testWriter, S[] sampleInitialWriters) {
//       foreach (S initialWriter in sampleInitialWriters)
//          testIdentity(testWriter, initialWriter);
//    }
//
//
//    private static void multitestComposition<S, A, B, C>(( Func<B, C> g,
//                                                           Func<A, B> h ) funcs,
//                                                         Writer<S, A> testWriter,
//                                                         S[] sampleInitialWriters) {
//       foreach (S initialWriter in sampleInitialWriters)
//          testComposition(funcs, testWriter, initialWriter);
//    }
// }
