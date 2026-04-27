using System;
using System.Linq;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;



namespace WelterKit.FunctionalContainers_tests.KList_tests;

[TestClass]
public class Laws_Functor {

   [TestMethod]
   public void Identity_Simple() {
      Laws.Functor.Identity(new KList<int>([1, 42, 999]),
                            collectionAssert_areEqual);
   }


   [TestMethod]
   public void Identity_Simple_Empty() {
      Laws.Functor.Identity(new KList<int>([]),
                            collectionAssert_areEqual);
   }


   [TestMethod]
   public void Composition_Simple() {
      Laws.Functor.Composition(new KList<int>([1, 42, 999]),
                               static (string x) => x + "$",
                               static (int x) => x.ToString(),
                               collectionAssert_areEqual);
   }


   [TestMethod]
   public void Composition_Simple_Empty() {
      Laws.Functor.Composition(new KList<int>([]),
                               static (string x) => x + "$",
                               static (int x) => x.ToString(),
                               collectionAssert_areEqual);
   }


   private static void collectionAssert_areEqual<A>(K<KList, A> fa, K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
