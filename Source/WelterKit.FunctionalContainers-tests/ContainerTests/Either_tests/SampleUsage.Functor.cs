using System;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public class SampleUsage_Functor {
   [TestMethod]
   public void FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";

      //TODO      print

      Assert.AreEqual(" 42/2 =  21.0",  ((Right<string, string>) Either.FMap(new Right<string, int>(42)              , sampleMap)).Value);
      Assert.AreEqual("previous error", ((Left <string, string>) Either.FMap(new Left <string, int>("previous error"), sampleMap)).Value);
   }


   [TestMethod]
   public void FMap_Sample2() {
      Either<string,int> divide(int x, int y) {
         (int quotient, int remainder) = int.DivRem(x, y);
         return remainder == 0
                      ? quotient                   // 'right' value
                      : $"{y} doesn't divide {x}"; // 'left' value
      }

      Assert.AreEqual(new Right<string, int>(15),                    divide(30, 2));
      Assert.AreEqual(new Left <string, int>("2 doesn't divide 15"), divide(15, 2));
   }


   // [TestMethod]
   // public void FMap_Chaining() {
   //    Either<string> findConfigFile(bool isValid)
   //       => isValid
   //                ? new Some<string>(@"C:\valid.configfile")
   //                : new None<string>();
   //
   //    Either<string> readConfiguration(string configFileName)
   //       => configFileName == @"C:\valid.configfile"
   //                ? new Some<string>("validConfig=true")
   //                : new None<string>();
   //
   //    // (potentially) returns server id
   //    Either<int> connectToServer(string configuration)
   //       => configuration == "validConfig=true"
   //                ? new Some<int>(4242)
   //                : new None<int>();
   //
   //    Either<string> a = findConfigFile(isValid: true);
   //          Either<string> b=a
   //                         .FMap<string, string>(readConfiguration).As();
   //                var c=b
   //           .FMap(connectToServer);
   //
   // }
}
