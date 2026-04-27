using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.KList_tests;

internal static class LawsTestHelpers {
   internal static void Multitest<A, B, C>(( KList<Func<B, C>> u,
                                             KList<Func<A, B>> v ) funcs,
                                           KList<A>[] values,
                                           Action<KList<Func<B, C>>,
                                                  KList<Func<A, B>>,
                                                  KList<A>         > testFuncsAndValue) {

      var uEmpty = new KList<Func<B, C>>([]);
      var vEmpty = new KList<Func<A, B>>([]);

      test(funcs.u, funcs.v);
      test(uEmpty , funcs.v);
      test(funcs.u, vEmpty );
      test(uEmpty , vEmpty );

      test(funcs.u.Dup(), funcs.v.Dup());
      test(uEmpty       , funcs.v.Dup());
      test(funcs.u.Dup(), vEmpty       );

      // TODO: test larger list and more elaborate combinations

      return;

      void test(KList<Func<B, C>> u, KList<Func<A, B>> v)
         => testValues(values,
                       testValue => testFuncsAndValue(u, v, testValue));

      static void testValues(KList<A>[] testValues, Action<KList<A>> test) {
         foreach (KList<A> testValue in testValues)
            test(testValue);
      }
   }


   internal static KList<A> Dup<A>(this KList<A> listToDuplicate)
      => new KList<A>(listToDuplicate.List.AddRange(
                      listToDuplicate.List));
}
