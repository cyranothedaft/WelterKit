using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests.Sorting {
   [TestClass]
   public class Test_MergeSort_Sort {
      [TestMethod]
      [Ignore("WIP")]
      public void Sort_Sample() {
         testSort(seq(2, 3, 1, 4, 5),
                  seq(2, 4, 3, 1, 5));
      }


      private void testSort(IList<int> expected, IList<int> toSort) {
         testSort(expected, toSort, Util.IntCompare);
      }


      private void testSort<T>(IList<T> expected, IList<T> toSort, Func<T, T, int> compareFunc) {
         Util.AssertCollection(expected,
                               MergeSort.Sort(toSort, compareFunc),
                               compareFunc);
      }


      private static IList<T> seq<T>(params T[] elements)
         => Util.Seq(elements);
   }
}
