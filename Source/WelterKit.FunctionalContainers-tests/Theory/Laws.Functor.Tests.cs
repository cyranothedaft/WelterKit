using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;
using WelterKit.StaticUtilities;


namespace WelterKit.FunctionalContainers_tests.Theory;

[TestClass]
public abstract class Laws_Functor_Tests<M> where M : IFunctor<M> {
   internal abstract ILawsTestData<M> TestData { get; }


   [TestMethod]
   public void Identity() {
      testIdentity(TestData.GetTestSubjects (TestValues._int    ), TestData.AssertAreEqual<int    >);
      testIdentity(TestData.GetTestSubjects (TestValues._float  ), TestData.AssertAreEqual<float  >);
      testIdentity(TestData.GetTestSubjects (TestValues._string ), TestData.AssertAreEqual<string >);
      testIdentity(TestData.GetTestSubjectsn(TestValues._stringn), TestData.AssertAreEqual<string?>);
      return;

      static void testIdentity<A>(K<M, A>[] testSubjects, Action<K<M, A>, K<M, A>> assertAreEqual)
         => testSubjects.ForEach((ma => Laws.Functor.Identity(ma, assertAreEqual)));
   }


   [TestMethod]
   public void Composition() {
      testComposition(TestData.GetTestSubjects (TestValues._int    ), TestFunctions.IntToStringToStringFuncs          , TestData.AssertAreEqual<string  >);
      testComposition(TestData.GetTestSubjects (TestValues._float  ), TestFunctions.FloatToIntToStringFuncs           , TestData.AssertAreEqual<string  >);
      testComposition(TestData.GetTestSubjects (TestValues._string ), TestFunctions.StringToIntToTimeSpanFuncs        , TestData.AssertAreEqual<TimeSpan>);
      testComposition(TestData.GetTestSubjectsn(TestValues._stringn), TestFunctions.StringNToBoolStringTupleToIntFuncs, TestData.AssertAreEqual<int     >);
      return;

      static void testComposition<A, B, C>(IEnumerable<K<M, A>> testSubjects, IEnumerable<(Func<A, B> h, Func<B, C> g)> testFuncs,
                                           Action<K<M, C>, K<M, C>> assertAreEqual)
         => testSubjects.SelectMany(_ => testFuncs, (subject, funcs) => (subject, funcs))
                        .ForEach(testInput => Laws.Functor.Composition(testInput.subject,
                                                                       testInput.funcs.g,
                                                                       testInput.funcs.h,
                                                                       assertAreEqual));
   }
}
