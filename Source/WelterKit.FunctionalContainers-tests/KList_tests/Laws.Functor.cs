using System;
using System.Linq;
using WelterKit.FunctionalContainers;


namespace WelterKit.FunctionalContainers_tests.KList_tests;

[TestClass]
public class Laws_Functor {

   private static T id<T>(T x) => x;


   // Functors must preserve identity
   [TestMethod]
   public void Identity_Simple() {
      KList<int> list = new KList<int>([1, 42, 999]);
      CollectionAssert.AreEqual(list.List,
                                ((KList<int>)KList.FMap(list, id)).List);
   }


   // Functors must preserve composition
   [TestMethod]
   public void Composition_Simple() {
      Func<string, string> f = x => x + "$";
      Func<int, string> g = x => x.ToString();

      KList<int> list = new KList<int>([1, 42, 999]);

      CollectionAssert.AreEqual(list.List.Select(g).Select(f).ToList(),
                                list.List.Select(x => f(g(x))).ToList());
   }

}
