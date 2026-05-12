using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;



namespace WelterKit.FunctionalContainers_tests.Theory;

[TestClass]
public abstract class Laws_Functor_Tests<M> where M : IFunctor<M> {

   protected abstract void AssertAreEqual<A>(K<M, A> expected, K<M, A> actual);
   protected abstract K<M, int>[] GetTestSubjects_int(int x);


   [TestMethod]
   public void Identity() {
      int[]intsToTest = [0, 42];

      IEnumerable<K<M, int>> intFunctors2test = from int i in intsToTest
                                                from K<M, int> t in GetTestSubjects_int(i)
                                                select t;

      // TODO: distinct?
      foreach (K<M, int> subject in intFunctors2test)
         Laws.Functor.Identity(subject, AssertAreEqual);

      // testIdentity(new None<float>());
      // testIdentity(new Some<float>(0));
      // testIdentity(new Some<float>(42.42f));
      // testIdentity(new None<string>());
      // testIdentity(new Some<string>(string.Empty));
      // testIdentity(new Some<string>("abc XYZ"));
      // testIdentity(new None<string?>());
      // testIdentity(new Some<string?>(null));
      // testIdentity(new Some<string?>("abc XYZ"));

      // TODO: more...

   }


   //
   //
   // [TestMethod]
   // public void Composition() {
   //    var funcs1 = ( g: (Func<string, string>)( static x => x + "$"   ),
   //                   h: (Func<int, string>   )( static x => x.ToString() ) );
   //
   //    (int state, int value) runReader1(int x) => (x, x);
   //    (int state, int value) runReader2(int x) => (x+1, x-1);
   //
   //    int[] initialReaders1 = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
   //
   //    multitestComposition(funcs1, new Reader<int, int>(runReader1), initialReaders1);
   //    multitestComposition(funcs1, new Reader<int, int>(runReader2), initialReaders1);
   ////    // TODO: more...
   // }
   //
   //
   // private void testIdentity<A>(K<M, A> testSubject)
   //    => Laws.Functor.Identity(testSubject, AssertAreEqual);
   //
   //
   // private static void testComposition<S, A, B, C>(( Func<B, C> g,
   //                                                   Func<A, B> h ) funcs,
   //                                                 Reader<S, A> testReader,
   //                                                 S sampleInitialReader)
   //    => Laws.Functor.Composition(testReader,
   //                                funcs.g,
   //                                funcs.h,
   //                                (expected, actual) => LawsTestHelpers.AssertReadersAreEqual(expected, actual, sampleInitialReader));
   //
   //
   // private static void multitestIdentity<S, A>(Reader<S, A> testReader, S[] sampleInitialReaders) {
   //    foreach (S initialReader in sampleInitialReaders)
   //       testIdentity(testReader, initialReader);
   // }
   //
   //
   // private static void multitestComposition<S, A, B, C>(( Func<B, C> g,
   //                                                        Func<A, B> h ) funcs,
   //                                                      Reader<S, A> testReader,
   //                                                      S[] sampleInitialReaders) {
   //    foreach (S initialReader in sampleInitialReaders)
   //       testComposition(funcs, testReader, initialReader);
   // }
}
