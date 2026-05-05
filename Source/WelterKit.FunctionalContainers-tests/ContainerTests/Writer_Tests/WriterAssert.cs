using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;

internal static class WriterAssert {
   public static void AreEqual<W, A>(K<Writer<W>, A> expected,
                                     K<Writer<W>, A> actual,
                                     Action<W, W> assertWriteesAreEqual) where W : IMonoid<W>
      => AreEqual(expected.As(), actual.As(), assertWriteesAreEqual);


   public static void AreEqual<W, A>(Writer<W, A> expected,
                                     Writer<W, A> actual,
                                     Action<W, W> assertWriteesAreEqual) where W : IMonoid<W> {
      (W writer, A value) exp = expected.RunWriter();
      (W writer, A value) act = actual  .RunWriter();

      Assert.AreEqual(exp.value, act.value);
      assertWriteesAreEqual(exp.writer, act.writer);
   }

   // partial function application
   public static Action<K<Writer<W>, A>, K<Writer<W>, A>> AreEqual<W, A>(Action<W, W> assertWriteesAreEqual) where W : IMonoid<W>
      => (expected, actual) => AreEqual(expected, actual, assertWriteesAreEqual);
}
