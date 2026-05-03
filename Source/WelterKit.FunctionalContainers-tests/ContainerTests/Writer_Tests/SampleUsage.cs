using System;
using System.Collections.Immutable;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;

[TestClass]
public class SampleUsage {

   [TestMethod]
   public void LogSampleApplicativeStyle() {

      // -- A writer action: log a calculation and return a value
      // logAndAdd :: Int -> Writer [String] Int
      // logAndAdd x = writer (x + 1, ["Added 1 to " ++ show x])
      // 
      // -- Using Applicative Style:
      // -- <$> (fmap) applies a binary function (e.g., +) to the first writer
      // -- <*> (apply) applies the second writer to the result
      // result :: Writer [String] Int
      // result = (+) <$> logAndAdd 1 <*> logAndAdd 10
      // 
      // -- Result:
      // -- runWriter result 
      // -- Output: (3, ["Added 1 to 1", "Added 1 to 10"])

      // result :: Writer [String] Int
      // result = (+) <$> logAndAdd 1 <*> logAndAdd 10

      // finalResult :: LoggedValue
      // finalResult = (*) <$> addOne 5 <*> multiplyTwo 3

//      Writer<KList<string>, int> result
//            = Writer<KList<string>>.FMap(
//                                    Writer<KList<string>>.Apply(addOne(5),multiplyTwo(3)),
//                                         addCurried
//                                         )
//
//      static int add(int x, int y) => x + y;
//      static Func<int, int> addCurried(int x) => y => x + y;
//
//      static Writer<KList<string>, int> addOne(int x)
//         => new ((new KList<string>([$"Added 1 to {x}"]),
//                  x + 1));
//
//      // multiplyTwo :: Int -> LoggedValue
//      // multiplyTwo x = writer (x * 2, ["Multiplied by 2"])
//
//      static Writer<KList<string>, int> multiplyTwo(int x)
//         => new ((new KList<string>(["Multiplied by 2"]),
//                  x * 2));


   }


   [TestMethod]
   public void LogSampleApplicativeStyle2() {
      // -- A function that returns a value and a log entry
      // logNumber :: Int -> Writer [String] Int
      // logNumber x = writer (x, ["Adding number: " ++ show x])


//      Writer<Logger, int> logNumber(int x)
//         => new((new Logger($"Adding number: {x}"),
//                 x));


      // -- Applicative style: Combines independent log entries automatically
      // -- pure (+) combines the results, while the log entries are concatenated
      // sumThreeLogs :: Writer [String] Int
      // sumThreeLogs = (+) <$> logNumber 10 <*> logNumber 20 <*> logNumber 30

      static int add(int x, int y) => x + y;
      Func<int, Func<int, int>> addCurried = Fn.Curry<int, int, int>(add);

//      Writer<Logger, int> sumThreeLogs()
//         => Writer<Logger>.FMap<int, int>(
//                                             logNumber(10)
//                                                  .Apply<Writer<Logger>, int, int>(logNumber(20))
//                                                  .Apply<Writer<Logger>, int, int>(logNumber(30)),
//                                             Writer<Logger>.Pure(addCurried)
//                                            );


      // step1 :: Writer [String] Int
      // step1 = writer (3, ["step1: produced 3"])

      Writer<Logger, int> step1() => new Writer<Logger, int>((new Logger(["step1: produced 3"]), 3));

      // step2 :: Writer [String] Int
      // step2 = writer (5, ["step2: produced 5"])

      Writer<Logger, int> step2() => new Writer<Logger, int>((new Logger(["step2: produced 5"]), 5));

      // combined :: Writer [String] Int
      // combined = pure (+) <*> step1 <*> step2

      Writer<Logger, int> combined()
         => Writer<Logger>.Pure(addCurried)
                          .Apply(step1())
                          .Apply(step2())
                          .As();

      // 
      // main :: IO ()
      // main = do
      //     let (result, logs) = runWriter sumThreeLogs
      //     putStrLn $ "Final Result: " ++ show result
      //     putStrLn "Logs:"
      //     mapM_ putStrLn logs

      (Logger logs, int result) = combined().runWriter;

      Assert.AreEqual(8, result);
      CollectionAssert.AreEqual(new []{"step1: produced 3", "step2: produced 5"}, logs.Entries);

   }

   private record Logger(ImmutableList<string> Entries) : IMonoid<Logger> {
      public Logger Combine(Logger rhs)
         => new(         this.Entries
                .AddRange(rhs.Entries));


      public static Logger Empty { get; }
         = new Logger([]);
   }
}
