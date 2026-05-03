using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;

[TestClass]
public class SampleUsage {

   [TestMethod]
   public void LogSampleApplicativeStyle() {
      // Arrange
      string[] expectedLogs =
         [
            "step1: [42] + [111] = [153]",
            "step2: [10] * 100 = [1000]"
         ];
      const int expectedResult = 153000;

      // Act
      (Logger logs, int result) = combined().RunWriter();

      // Assert
      Assert.AreEqual(expectedResult, result);
      CollectionAssert.AreEqual(expectedLogs, logs.Entries);
      return;


      static Writer<Logger, int> step1(int a, int b) {
         int result = a + b;
         string logMsg = $"{nameof( step1 )}: [{a}] + [{b}] = [{result}]";
         return new Writer<Logger, int>(() => (new Logger([logMsg]), result));
      }

      static Writer<Logger, int> step2(int a) {
         int result = a * 100;
         string logMsg = $"{nameof( step2 )}: [{a}] * 100 = [{result}]";
         return new Writer<Logger, int>(() => (new Logger([logMsg]), result));
      }

      Writer<Logger, int> combined()
         => Writer<Logger>.Pure(Fn.Curry<int, int, int>(multiply)) // combine step results by multiplying them
                           // note: these are independent operations:
                          .Apply(step1(42, 111))
                          .Apply(step2(10))
                          .As();

      static int multiply(int x, int y) => x * y;
   }

   private record Logger(ImmutableList<string> Entries) : IMonoid<Logger> {
      public Logger Combine(Logger rhs)
         => new(         this.Entries
                .AddRange(rhs.Entries));


      public static Logger Empty { get; }
         = new Logger([]);
   }
}
