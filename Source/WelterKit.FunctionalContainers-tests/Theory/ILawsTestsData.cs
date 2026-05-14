using System;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.Theory;

internal interface ILawsTestData<M> {
   // TODO: static abstract?
   void AssertAreEqual<A>(K<M, A> expected, K<M, A> actual);

   K<M, int    >[] GetTestSubjects (int    [] x);
   K<M, float  >[] GetTestSubjects (float  [] x);
   K<M, string >[] GetTestSubjects (string [] x);
   K<M, string?>[] GetTestSubjectsn(string?[] x);
}
