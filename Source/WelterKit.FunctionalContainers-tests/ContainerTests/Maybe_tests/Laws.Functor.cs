using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Maybe_tests;

[TestClass]
public class Laws_Functor : Laws_Functor_Tests<Maybe> {
   protected override void AssertAreEqual<A>(K<Maybe, A> expected, K<Maybe, A> actual)
      => Assert.AreEqual(expected.As(), actual.As());

   protected override K<Maybe, int    >[] GetTestSubjects_int    (int     x) => someAndNone(x);
   protected override K<Maybe, float  >[] GetTestSubjects_float  (float   x) => someAndNone(x);
   protected override K<Maybe, string >[] GetTestSubjects_string (string  x) => someAndNone(x);
   protected override K<Maybe, string?>[] GetTestSubjects_stringn(string? x) => someAndNone(x);

   private static K<Maybe, A>[] someAndNone<A>(A x) => [new Some<A>(x), new None<A>()];
}
