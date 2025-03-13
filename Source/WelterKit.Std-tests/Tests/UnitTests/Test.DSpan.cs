using System;
using System.Collections.Generic;
using WelterKit.Std;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests.Tests.UnitTests {
   [TestClass]
   public class Test_DSpan {
      [TestMethod]
      public void Length_Sample() {
         Assert.AreEqual(4, new DSpan<int>(seq(1, 2, 3, 4, 5, 6, 7, 8, 9), 3, 4)
                            .Length);
      }


      [TestMethod]
      public void ToList_Sample() {
         Util.AssertCollection(seq(4, 5, 6, 7),
                               new DSpan<int>(seq(1, 2, 3, 4, 5, 6, 7, 8, 9), 3, 4)
                                  .ToList(),
                               Util.IntCompare);
      }


      private static IList<T> seq<T>(params T[] elements)
         => Util.Seq(elements);
   }
}
