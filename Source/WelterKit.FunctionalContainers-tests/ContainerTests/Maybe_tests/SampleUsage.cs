using System;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Maybe_tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";

      //TODO      print

      Assert.AreEqual(" 42/2 =  21.0", ((Some<string>) Maybe.FMap(new Some<int>(42), sampleMap)).Value);
      Assert.IsInstanceOfType<           None<string>>(Maybe.FMap(new None<int>()  , sampleMap));
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
}
