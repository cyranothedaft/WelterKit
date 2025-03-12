using System;
using System.Collections.Generic;
using WelterKit.Std.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests.Sorting {
   [TestClass]
   public class Test_MergeSort_Merge {
      [TestMethod]
      public void Merge_Sample() {
         testMerge(seq(2, 3, 1, 4, 5),
                   seq(2, 4),
                   seq(3, 1, 5));
      }


      [TestMethod]
      public void Merge_Empty() {
         testMerge(Array.Empty<int>(), Array.Empty<int>(), Array.Empty<int>());
         testMerge(seq(1)   , seq(1)           , Array.Empty<int>());
         testMerge(seq(1, 2), seq(1, 2)        , Array.Empty<int>());
         testMerge(seq(1)   , Array.Empty<int>(), seq(1));
         testMerge(seq(1, 2), Array.Empty<int>(), seq(1, 2));
      }


      [TestMethod]
      public void Merge_IdenticalLists() {
         testFor(seq( 1, 1 ),             seq(1));
         testFor(seq( 1, 1, 1, 1 ),       seq(1, 1));
         testFor(seq( 1, 1, 1, 1, 1, 1 ), seq(1, 1, 1));
         testFor(seq( 1, 1, 2, 2 ),       seq(1, 2));
         testFor(seq( 2, 1, 2, 1 ),       seq(2, 1));
         testFor(seq( 1, 1, 2, 2, 3, 3 ), seq(1, 2, 3));
         testFor(seq( 3, 2, 1, 3, 2, 1 ), seq(3, 2, 1));

         void testFor(IList<int> expected, IList<int> list1and2) {
            testMerge(expected, list1and2, list1and2);
         }
      }


      [TestMethod]
      public void Merge_DistinctElements() {
         testMerge(seq( 1, 2 ),       seq( 1 ),    seq( 2 ));
         testMerge(seq( 1, 2, 3, 4 ), seq( 1, 2 ), seq( 3, 4 ));
         testMerge(seq( 3, 1, 4, 2 ), seq( 3, 1 ), seq( 4, 2 ));
         testMerge(seq( 1, 2, 3, 4 ), seq( 2, 4 ), seq( 1, 3 ));
      }


      [TestMethod]
      public void Merge_DifferingListSizes() {
         testMerge(seq( 1, 1, 1 ),       seq( 1 )      , seq( 1, 1 ));
         testMerge(seq( 1, 1, 1, 1 ),    seq( 1 )      , seq( 1, 1, 1 ));
         testMerge(seq( 1, 1, 1 ),       seq( 1, 1 )   , seq( 1 ));
         testMerge(seq( 1, 1, 1, 1, 1 ), seq( 1, 1 )   , seq( 1, 1, 1 ));
         testMerge(seq( 1, 1, 1, 1 ),    seq( 1, 1, 1 ), seq( 1 ));
         testMerge(seq( 1, 1, 1, 1, 1 ), seq( 1, 1, 1 ), seq( 1, 1 ));

         testMerge(seq( 2, 3, 4 ),       seq( 2 )      , seq( 3, 4 ));
         testMerge(seq( 2, 3, 4, 5 ),    seq( 2 )      , seq( 3, 4, 5 ));
         testMerge(seq( 2, 4, 3 ),       seq( 4, 3 )   , seq( 2 ));
         testMerge(seq( 2, 3, 4, 5, 6 ), seq( 2, 3 )   , seq( 4, 5, 6 ));
         testMerge(seq( 2, 5, 4, 3 ),    seq( 5, 4, 3 ), seq( 2 ));
         testMerge(seq( 3, 2, 6, 5, 4 ), seq( 6, 5, 4 ), seq( 3, 2 ));
      }


      private void testMerge(IList<int> expected, IList<int> list1, IList<int> list2) {
         testMerge(expected, list1, list2, Util.IntCompare);
      }


      private void testMerge<T>(IList<T> expected, IList<T> list1, IList<T> list2,
                                Func<T, T, int> compareFunc) {
         Util.AssertCollection(expected,
                               MergeSort.Merge(list1, list2, compareFunc),
                               compareFunc);
      }


      private static IList<T> seq<T>(params T[] elements)
         => Util.Seq(elements);
   }
}
