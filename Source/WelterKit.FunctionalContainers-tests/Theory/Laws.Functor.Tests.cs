using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;
using WelterKit.StaticUtilities;


namespace WelterKit.FunctionalContainers_tests.Theory;

[TestClass]
public abstract class Laws_Functor_Tests<M> where M : IFunctor<M> {

   protected abstract void AssertAreEqual<A>(K<M, A> expected, K<M, A> actual);

   protected abstract K<M, int    >[] GetTestSubjects_int    (int     x);
   protected abstract K<M, float  >[] GetTestSubjects_float  (float   x);
   protected abstract K<M, string >[] GetTestSubjects_string (string  x);
   protected abstract K<M, string?>[] GetTestSubjects_stringn(string? x);


   [TestMethod]
   public void Identity() {
      testIdentity(TestValues._int    , GetTestSubjects_int    , AssertAreEqual);
      testIdentity(TestValues._float  , GetTestSubjects_float  , AssertAreEqual);
      testIdentity(TestValues._string , GetTestSubjects_string , AssertAreEqual);
      testIdentity(TestValues._stringn, GetTestSubjects_stringn, AssertAreEqual);
      return;

      static void testIdentity<A>(IEnumerable<A> testValues,
                                  Func<A, IEnumerable<K<M, A>>> getTestSubjects,
                                  Action<K<M, A>, K<M, A>> assertAreEqual)
         => testValues.SelectMany(getTestSubjects)
                      .Distinct()
                      .ForEach(subject => Laws.Functor.Identity(subject, assertAreEqual));
   }


   [TestMethod]
   public void Composition() {
      testComposition(TestValues._int    , TestFunctions.IntToStringToStringFuncs          , GetTestSubjects_int    , AssertAreEqual<string  >);
      testComposition(TestValues._float  , TestFunctions.FloatToIntToStringFuncs           , GetTestSubjects_float  , AssertAreEqual<string  >);
      testComposition(TestValues._string , TestFunctions.StringToIntToTimeSpanFuncs        , GetTestSubjects_string , AssertAreEqual<TimeSpan>);
      testComposition(TestValues._stringn, TestFunctions.StringNToBoolStringTupleToIntFuncs, GetTestSubjects_stringn, AssertAreEqual<int     >);
      return;

      static void testComposition<A, B, C>(IEnumerable<A> testValues, IEnumerable<(Func<A, B> h, Func<B, C> g)> testFuncs,
                                           Func<A, IEnumerable<K<M, A>>> getTestSubjects,
                                           Action<K<M, C>, K<M, C>> assertAreEqual)
         => testValues.SelectMany(getTestSubjects)
                      .Distinct()
                      .SelectMany(_ => testFuncs, (subject, funcs) => (subject, funcs))
                      .ForEach(testInput => Laws.Functor.Composition(testInput.subject,
                                                                     testInput.funcs.g,
                                                                     testInput.funcs.h,
                                                                     assertAreEqual));
   }
}
