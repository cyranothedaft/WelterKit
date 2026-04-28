using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public class SampleUsage_Functor {
   [TestMethod]
   public void FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";

      //TODO      print

      Assert.AreEqual(" 42/2 =  21.0",  ((Right<string, string>) Either<string>.FMap(new Right<string, int>(42)              , sampleMap)).Value);
      Assert.AreEqual("previous error", ((Left <string, string>) Either<string>.FMap(new Left <string, int>("previous error"), sampleMap)).Value);
   }


   [TestMethod]
   public void Sample2() {
      Either<string, int> divide(int x, int y) {
         (int quotient, int remainder) = int.DivRem(x, y);
         return remainder == 0
                      ? quotient                   // 'right' value
                      : $"{y} doesn't divide {x}"; // 'left' value
      }

      Assert.AreEqual(new Right<string, int>(15),                    divide(30, 2));
      Assert.AreEqual(new Left <string, int>("2 doesn't divide 15"), divide(15, 2));
   }


   [TestMethod]
   public void Chaining() {
      const string ConfigFileName = @"C:\configfile.cfg";
      const string ConfigurationContents = "validConfig=true;connectToServerThatWorks=true";
      const int ServerId = 4242;
      const float QueryResult = 42.42f;


      CustomAssert.IsRight(QueryResult, test(true , true , true , true ));
      CustomAssert.IsLeft($"ERROR: failed to run query on server {ServerId}"                                      , test(true , true , true , false));
      CustomAssert.IsLeft($"ERROR: failed to connect to server using this configuration:  {ConfigurationContents}", test(true , true , false, true ));
      CustomAssert.IsLeft($"ERROR: failed to connect to server using this configuration:  {ConfigurationContents}", test(true , true , false, false));
      CustomAssert.IsLeft($"ERROR: config file is invalid:  {ConfigFileName}"                                     , test(true , false, true , true ));
      CustomAssert.IsLeft($"ERROR: config file is invalid:  {ConfigFileName}"                                     , test(true , false, true , false));
      CustomAssert.IsLeft($"ERROR: config file is invalid:  {ConfigFileName}"                                     , test(true , false, false, true ));
      CustomAssert.IsLeft($"ERROR: config file is invalid:  {ConfigFileName}"                                     , test(true , false, false, false));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, true , true , true ));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, true , true , false));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, true , false, true ));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, true , false, false));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, false, true , true ));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, false, true , false));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, false, false, true ));
      CustomAssert.IsLeft("ERROR: config file not found"                                                          , test(false, false, false, false));
      return;

      static Either<string, float> test(bool isConfigFileFound,
                                        bool isConfigFileValid,
                                        bool doesServerConnectionSucceed,
                                        bool doesQuerySucceed) {
         return      findConfigFile()
               .Bind(readConfiguration)
               .Bind(connectToServer)
               .Bind(runQuery)
               .As();

         Either<string, string> findConfigFile()
            => isConfigFileFound
                     ? ConfigFileName.AsRight<string, string>()
                     : "ERROR: config file not found".AsLeft<string, string>();

         Either<string, string> readConfiguration(string configFileName)
            => isConfigFileValid
                     ? ConfigurationContents.AsRight<string, string>()
                     : $"ERROR: config file is invalid:  {configFileName}".AsLeft<string, string>();

         // (potentially) returns a "server id"
         Either<string, int> connectToServer(string configuration)
            => doesServerConnectionSucceed
                     ? ServerId
                     : $"ERROR: failed to connect to server using this configuration:  {configuration}";

         Either<string, float> runQuery(int serverId)
            => doesQuerySucceed
                     ? QueryResult
                     : $"ERROR: failed to run query on server {serverId}";
      }
   }
}



internal static class CustomAssert {
   public static void IsLeft<L, R>(L expectedLeftValue, Either<L, R> actualEither) {
      Assert.IsInstanceOfType<Left<L, R>>(actualEither);
      Assert.AreEqual(expectedLeftValue, ((Left<L, R>)actualEither).Value);
   }


   public static void IsRight<L, R>(R expectedRightValue, Either<L, R> actualEither) {
      Assert.IsInstanceOfType<Right<L, R>>(actualEither);
      Assert.AreEqual(expectedRightValue, ((Right<L, R>)actualEither).Value);
   }
}
