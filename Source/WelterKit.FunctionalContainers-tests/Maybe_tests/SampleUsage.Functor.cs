using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.Maybe_tests;

[TestClass]
public class SampleUsage_Functor {
   [TestMethod]
   public void FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";

      Maybe<int> maybe = new Some<int>(42);

      //TODO      print

      Assert.AreEqual(" 42/2 =  21.0", ((Some<string>)Maybe.FMap(maybe, sampleMap)).Value);
   }


   [TestMethod]
   public void FMap_Sample2() {
      Maybe<int> divide(int x, int y) {
         (int quotient, int remainder) = int.DivRem(x, y);
         return remainder == 0
                      ? new Some<int>(quotient)
                      : new None<int>();
      }

      Assert.AreEqual(new Some<int>(15), divide(30, 2));
      Assert.AreEqual(new None<int>()  , divide(15, 2));
   }


   // [TestMethod]
   // public void FMap_Chaining() {
   //    Maybe<string> findConfigFile(bool isValid)
   //       => isValid
   //                ? new Some<string>(@"C:\valid.configfile")
   //                : new None<string>();
   //
   //    Maybe<string> readConfiguration(string configFileName)
   //       => configFileName == @"C:\valid.configfile"
   //                ? new Some<string>("validConfig=true")
   //                : new None<string>();
   //
   //    // (potentially) returns server id
   //    Maybe<int> connectToServer(string configuration)
   //       => configuration == "validConfig=true"
   //                ? new Some<int>(4242)
   //                : new None<int>();
   //
   //    Maybe<string> a = findConfigFile(isValid: true);
   //          Maybe<string> b=a
   //                         .FMap<string, string>(readConfiguration).As();
   //                var c=b
   //           .FMap(connectToServer);
   //
   // }
}
