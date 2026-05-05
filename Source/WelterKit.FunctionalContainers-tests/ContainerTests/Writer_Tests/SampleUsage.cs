using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;
using static WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests.Helpers;


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
      (StringAccumulator logs, int result) = combined().RunWriter();

      // Assert
      Assert.AreEqual(expectedResult, result);
      CollectionAssert.AreEqual(expectedLogs, logs.Entries);
      return;


      Writer<StringAccumulator, int> combined()
         => Writer<StringAccumulator>.Pure(Fn.Curry<int, int, int>(multiply)) // combine step results by multiplying them
                                      // note: these are independent operations:
                                     .Apply(step1(42, 111))
                                     .Apply(step2(10))
                                     .As();

      static Writer<StringAccumulator, int> step1(int a, int b) {
         int result = a + b;
         string logMsg = $"{nameof( step1 )}: [{a}] + [{b}] = [{result}]";
         return new Writer<StringAccumulator, int>(() => (new StringAccumulator([logMsg]), result));
      }

      static Writer<StringAccumulator, int> step2(int a) {
         int result = a * 100;
         string logMsg = $"{nameof( step2 )}: [{a}] * 100 = [{result}]";
         return new Writer<StringAccumulator, int>(() => (new StringAccumulator([logMsg]), result));
      }

      static int multiply(int x, int y) => x * y;
   }


   [TestMethod]
   public void LogSampleMonadStyle() {
      // -- A simple function that logs its input
      // logValue :: Int -> Writer [String] Int
      // logValue x = writer (x, ["Logged: " ++ show x])
      // 
      // -- Chaining without do notation
      // multWithLog :: Writer [String] Int
      // multWithLog = 
      //     logValue 3 >>= \a ->
      //     logValue 5 >>= \b ->
      //     return (a * b)
      // 
      // -- Running the computation
      // -- runWriter multWithLog returns (15, ["Logged: 3", "Logged: 5"])

      (StringAccumulator writer, int value) finalResult = doSomeMultiplying().As().RunWriter();
      Assert.AreEqual(3*5, finalResult.value);
      StringAccumulatorAssert.AreEqual(new(["Logged: 3", "Logged: 5"]), finalResult.writer);
      return;


      static Writer<StringAccumulator, int> logValue(int value)
         => NewWriter([$"Logged: {value}"], value);

      static K<Writer<StringAccumulator>, int> doSomeMultiplying()
         => logValue(3)
              .Bind(a => logValue(5)
              .Bind(b => (a * b)
              .Return<Writer<StringAccumulator>, int>()
               ));
   }
}
