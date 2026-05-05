using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Traits;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.KList_tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void Functor_FMap_Sample() {
      string sampleMap(int num) => $"{num,3}/2 = {(decimal)num / 2,5:###.0}";
      KList<int> list = new KList<int>([1, 42, 999]);

      KList<string> newList = KList.FMap(list, sampleMap).As();

      //TODO      print

      CollectionAssert.AreEqual(new[]
                                   {
                                      "  1/2 =    .5",
                                      " 42/2 =  21.0",
                                      "999/2 = 499.5",
                                   },
                                newList.List);
   }


   [TestMethod]
   public void Foldable_Foldr_Sample() {
      int result = new KList<int>([1, 2, 3]).Foldr(subtract, 0);

      Assert.AreEqual(1 - (2 - (3 - 0)), result); // == 2
      return;

      int subtract(int x, int y) => x - y;
   }
}
