using System;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.KList_tests;

[TestClass]
public class SampleUsage_Functor {
   [TestMethod]
   public void FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";
      KList<int> list = new KList<int>([1, 42, 999]);

      KList<string> newList = (KList<string>)KList.FMap(list, sampleMap);

//TODO      print

      CollectionAssert.AreEqual(new[]
                                   {
                                      "  1/2 =    .5",
                                      " 42/2 =  21.0",
                                      "999/2 = 499.5",
                                   },
                                newList.List);
   }
}
