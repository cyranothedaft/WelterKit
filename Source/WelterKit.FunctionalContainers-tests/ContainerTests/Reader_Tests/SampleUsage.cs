using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests.Helpers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Reader_Tests;

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
      (StringAccumulator logs, int result) = combined().RunReader();

      // Assert
      Assert.AreEqual(expectedResult, result);
      CollectionAssert.AreEqual(expectedLogs, logs.Entries);
      return;


      Reader<StringAccumulator, int> combined()
         => Reader<StringAccumulator>.Pure(Fn.Curry<int, int, int>(multiply)) // combine step results by multiplying them
                                      // note: these are independent operations:
                                     .Apply(step1(42, 111))
                                     .Apply(step2(10))
                                     .As();

      static Reader<StringAccumulator, int> step1(int a, int b) {
         int result = a + b;
         string logMsg = $"{nameof( step1 )}: [{a}] + [{b}] = [{result}]";
         return new Reader<StringAccumulator, int>(() => (new StringAccumulator([logMsg]), result));
      }

      static Reader<StringAccumulator, int> step2(int a) {
         int result = a * 100;
         string logMsg = $"{nameof( step2 )}: [{a}] * 100 = [{result}]";
         return new Reader<StringAccumulator, int>(() => (new StringAccumulator([logMsg]), result));
      }

      static int multiply(int x, int y) => x * y;
   }


   [TestMethod]
   public void LogSampleMonadStyle() {
      // -- A simple function that logs its input
      // logValue :: Int -> Reader [String] Int
      // logValue x = writer (x, ["Logged: " ++ show x])
      // 
      // -- Chaining without do notation
      // multWithLog :: Reader [String] Int
      // multWithLog = 
      //     logValue 3 >>= \a ->
      //     logValue 5 >>= \b ->
      //     return (a * b)
      // 
      // -- Running the computation
      // -- runReader multWithLog returns (15, ["Logged: 3", "Logged: 5"])

      (StringAccumulator writer, int value) finalResult = doSomeMultiplying().As().RunReader();
      Assert.AreEqual(3*5, finalResult.value);
      StringAccumulatorAssert.AreEqual(new(["Logged: 3", "Logged: 5"]), finalResult.writer);
      return;


      static Reader<StringAccumulator, int> logValue(int value)
         => NewReader([$"Logged: {value}"], value);

      static K<Reader<StringAccumulator>, int> doSomeMultiplying()
         => logValue(3)
              .Bind(a => logValue(5)
              .Bind(b => (a * b)
              .Return<Reader<StringAccumulator>, int>()
               ));
   }
}
