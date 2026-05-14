using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;
using WelterKit.StaticUtilities;


namespace WelterKit.FunctionalContainers_tests.Theory;

[TestClass]
public abstract class Laws_Applicative_Tests<M> where M : IApplicative<M> {

   protected abstract void AssertAreEqual<A>(K<M, A> expected, K<M, A> actual);

   protected abstract K<M, int    >[] GetTestSubjects (int    [] x);
   protected abstract K<M, float  >[] GetTestSubjects (float  [] x);
   protected abstract K<M, string >[] GetTestSubjects (string [] x);
   protected abstract K<M, string?>[] GetTestSubjectsn(string?[] x);


   [TestMethod]
   public void Identity() {
      testIdentity(GetTestSubjects (TestValues._int    ), AssertAreEqual<int    >);
      testIdentity(GetTestSubjects (TestValues._float  ), AssertAreEqual<float  >);
      testIdentity(GetTestSubjects (TestValues._string ), AssertAreEqual<string >);
      testIdentity(GetTestSubjectsn(TestValues._stringn), AssertAreEqual<string?>);
      return;

      static void testIdentity<A>(K<M, A>[] testSubjects, Action<K<M, A>, K<M, A>> assertAreEqual)
         => testSubjects.ForEach((ma => Laws.Applicative.Identity(ma, assertAreEqual)));
   }




   [TestMethod]
   public void Composition() {
      testComposition(GetTestSubjects (TestValues._int    ), TestFunctions.IntToStringToStringFuncs          .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._float  ), TestFunctions.FloatToIntToStringFuncs           .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._string ), TestFunctions.StringToIntToTimeSpanFuncs        .Select(pure), AssertAreEqual<TimeSpan>);
      testComposition(GetTestSubjectsn(TestValues._stringn), TestFunctions.StringNToBoolStringTupleToIntFuncs.Select(pure), AssertAreEqual<int     >);
      return;

      static void testComposition<A, B, C>(IEnumerable<K<M, A>> testSubjects, IEnumerable<(K<M, Func<A, B>> v, K<M, Func<B, C>> u)> testFuncs,
                                           Action<K<M, C>, K<M, C>> assertAreEqual)
         => testSubjects.SelectMany(_ => testFuncs, (subject, funcs) => (subject, funcs))
                        .ForEach(testInput => Laws.Applicative.Composition(testInput.funcs.u,
                                                                           testInput.funcs.v,
                                                                           testInput.subject,
                                                                           assertAreEqual));
   }


   [TestMethod]
   public void Homomorphism() {
      testComposition(GetTestSubjects (TestValues._int    ), TestFunctions.IntToStringToStringFuncs          .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._float  ), TestFunctions.FloatToIntToStringFuncs           .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._string ), TestFunctions.StringToIntToTimeSpanFuncs        .Select(pure), AssertAreEqual<TimeSpan>);
      testComposition(GetTestSubjectsn(TestValues._stringn), TestFunctions.StringNToBoolStringTupleToIntFuncs.Select(pure), AssertAreEqual<int     >);
      return;

      static void testComposition<A, B, C>(IEnumerable<K<M, A>> testSubjects, IEnumerable<(K<M, Func<A, B>> v, K<M, Func<B, C>> u)> testFuncs,
                                           Action<K<M, C>, K<M, C>> assertAreEqual)
         => testSubjects.SelectMany(_ => testFuncs, (subject, funcs) => (subject, funcs))
                        .ForEach(testInput => Laws.Applicative.Composition(testInput.funcs.u,
                                                                           testInput.funcs.v,
                                                                           testInput.subject,
                                                                           assertAreEqual));
   }


   [TestMethod]
   public void Interchange() {
      testComposition(GetTestSubjects (TestValues._int    ), TestFunctions.IntToStringToStringFuncs          .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._float  ), TestFunctions.FloatToIntToStringFuncs           .Select(pure), AssertAreEqual<string  >);
      testComposition(GetTestSubjects (TestValues._string ), TestFunctions.StringToIntToTimeSpanFuncs        .Select(pure), AssertAreEqual<TimeSpan>);
      testComposition(GetTestSubjectsn(TestValues._stringn), TestFunctions.StringNToBoolStringTupleToIntFuncs.Select(pure), AssertAreEqual<int     >);
      return;

      static void testComposition<A, B, C>(IEnumerable<K<M, A>> testSubjects, IEnumerable<(K<M, Func<A, B>> v, K<M, Func<B, C>> u)> testFuncs,
                                           Action<K<M, C>, K<M, C>> assertAreEqual)
         => testSubjects.SelectMany(_ => testFuncs, (subject, funcs) => (subject, funcs))
                        .ForEach(testInput => Laws.Applicative.Composition(testInput.funcs.u,
                                                                           testInput.funcs.v,
                                                                           testInput.subject,
                                                                           assertAreEqual));
   }


   private static (K<M, T1>, K<M, T2>) pure<T1, T2>((T1 a, T2 b) tuple)
      => (M.Pure(tuple.a),
          M.Pure(tuple.b));
}
