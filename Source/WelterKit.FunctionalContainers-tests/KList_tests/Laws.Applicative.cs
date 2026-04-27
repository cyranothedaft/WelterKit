using System;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Framework;



namespace WelterKit.FunctionalContainers_tests.KList_tests;


[TestClass]
public class Laws_Applicative {

   [TestMethod]
   public void Identity_Simple() {
      Laws.Applicative.Identity(new KList<int>([1, 42, 999]),
                                collectionAssert_areEqual);
   }


   [TestMethod]
   public void Identity_Empty() {
      Laws.Applicative.Identity(new KList<int>([]),
                                collectionAssert_areEqual);
   }


   [TestMethod]
   public void Composition_Simple() {
      Laws.Applicative.Composition(KList.Pure(static (string x) => x + "$"),
                                   KList.Pure(static (int x) => x.ToString()),
                                   new KList<int>([1, 42, 999]),
                                   collectionAssert_areEqual);
   }


   [TestMethod]
   public void Composition_Simple_Empty() {
      Laws.Applicative.Composition(KList.Pure(static (string x) => x + "$"),
                                   KList.Pure(static (int x) => x.ToString()),
                                   new KList<int>([]),
                                   collectionAssert_areEqual);
   }



   private static void collectionAssert_areEqual<A>(K<KList, A> fa, K<KList, A> fb)
      => CollectionAssert.AreEqual(fa.As().List,
                                   fb.As().List);
}
