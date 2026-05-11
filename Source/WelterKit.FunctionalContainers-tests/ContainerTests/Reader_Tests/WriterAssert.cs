using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_tests;

internal static class ReaderAssert {
   public static void AreEqual<W, A>(K<Reader<W>, A> expected,
                                     K<Reader<W>, A> actual,
                                     Action<W, W> assertWriteesAreEqual) where W : IMonoid<W>
      => AreEqual(expected.As(), actual.As(), assertWriteesAreEqual);


   public static void AreEqual<W, A>(Reader<W, A> expected,
                                     Reader<W, A> actual,
                                     Action<W, W> assertWriteesAreEqual) where W : IMonoid<W> {
      (W writer, A value) exp = expected.RunReader();
      (W writer, A value) act = actual  .RunReader();

      Assert.AreEqual(exp.value, act.value);
      assertWriteesAreEqual(exp.writer, act.writer);
   }

   // partial function application
   public static Action<K<Reader<W>, A>, K<Reader<W>, A>> AreEqual<W, A>(Action<W, W> assertWriteesAreEqual) where W : IMonoid<W>
      => (expected, actual) => AreEqual(expected, actual, assertWriteesAreEqual);
}
