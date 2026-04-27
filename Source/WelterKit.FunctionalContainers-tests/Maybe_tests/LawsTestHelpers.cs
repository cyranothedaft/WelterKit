using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;

internal static class LawsTestHelpers {
   internal static void Multitest<A, B, C>(( Maybe<Func<B, C>> u,
                                             Maybe<Func<A, B>> v ) funcs,
                                           Maybe<A>[] values,
                                           Action<Maybe<Func<B, C>>,
                                                  Maybe<Func<A, B>>,
                                                  Maybe<A>> testFuncsAndValue) {
      var uNone = new None<Func<B, C>>();
      var vNone = new None<Func<A, B>>();

      test(funcs.u, funcs.v);
      test(uNone,   funcs.v);
      test(funcs.u, vNone);
      test(uNone,   vNone);

      return;

      void test(Maybe<Func<B, C>> u, Maybe<Func<A, B>> v)
         => testValues(values,
                       testValue => testFuncsAndValue(u, v, testValue));

      static void testValues(Maybe<A>[] testValues, Action<Maybe<A>> test) {
         foreach (Maybe<A> testValue in testValues)
            test(testValue);
      }
   }
}
