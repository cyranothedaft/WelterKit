using System;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.Either_tests;

internal static class LawsTestHelpers {
   internal static void Multitest<L, A, B, C>(( Either<L, Func<B, C>> u,
                                             Either<L, Func<A, B>> v ) funcs,
                                           Either<L, A>[] values,
                                           Action<Either<L, Func<B, C>>,
                                                  Either<L, Func<A, B>>,
                                                  Either<L, A>> testFuncsAndValue,
                                           L valueIfLeft) {
      var uLeft = new Left<L, Func<B, C>>(valueIfLeft);
      var vLeft = new Left<L, Func<A, B>>(valueIfLeft);

      test(funcs.u, funcs.v);
      test(uLeft,   funcs.v);
      test(funcs.u, vLeft);
      test(uLeft,   vLeft);

      return;

      void test(Either<L, Func<B, C>> u, Either<L, Func<A, B>> v)
         => testValues(values,
                       testValue => testFuncsAndValue(u, v, testValue));

      static void testValues(Either<L, A>[] testValues, Action<Either<L, A>> test) {
         foreach (Either<L, A> testValue in testValues)
            test(testValue);
      }
   }
}
