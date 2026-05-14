using System;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Maybe_tests;

[TestClass]
public class Laws_Functor : Laws_Functor_Tests<Maybe> {
   protected override void AssertAreEqual<A>(K<Maybe, A> expected, K<Maybe, A> actual)
      => Assert.AreEqual(expected.As(),
                         actual  .As());


   protected override K<Maybe, int    >[] GetTestSubjects (int    [] values) => someAndNone(values);
   protected override K<Maybe, float  >[] GetTestSubjects (float  [] values) => someAndNone(values);
   protected override K<Maybe, string >[] GetTestSubjects (string [] values) => someAndNone(values);
   protected override K<Maybe, string?>[] GetTestSubjectsn(string?[] values) => someAndNone(values);


   private static K<Maybe, A>[] someAndNone<A>(A[] values)
      => (from x in values
          from variants in someAndNone(x)
          select variants
         ).ToArray();


   private static K<Maybe, A>[] someAndNone<A>(A x) 
      => [new Some<A>(x), new None<A>()];
}
