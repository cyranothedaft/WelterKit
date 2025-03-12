using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std;
using WelterKit.Std.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests.Sorting {
   [TestClass]
   public class Test {
      [TestMethod]
      [Ignore("WIP")]
      public void Divide_Sample() {
         Util.AssertCollection(seq<IList<int>>(seq(1, 2, 3), seq(4, 5)),
                               MergeSort.Divide(seq(1, 2, 3, 4, 5).ToSpan())
                                        .Select(span => span.ToList())
                                        .ToList(),
                               compareSeq, listToString);
      }


      [TestMethod]
      public void Divide_Empty() {
         Util.AssertCollection(seq<IList<int>>(),
                               MergeSort.Divide(DSpan<int>.Empty)
                                        .Select(span => span.ToList())
                                        .ToList(),
                               compareSeq, listToString);
      }


      private int compareSeq(IList<int> x, IList<int> y) {
         Util.AssertCollection(x, y, Util.IntCompare);
         return 0;
      }


      private static string listToString(IList<int> list)
         => "{" + string.Join(", ", list.Select(x => x.ToString())) + "}";


      private static List<T> seq<T>() => Util.Seq<T>();
      private static List<T> seq<T>(params T[] elements) => Util.Seq(elements);
   }
}
