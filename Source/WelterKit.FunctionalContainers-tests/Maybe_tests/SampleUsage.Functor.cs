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
}
