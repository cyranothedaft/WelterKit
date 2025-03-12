using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;


namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_EnumerableUtil {
      #region Chain

      [TestMethod]
      public void Chain_sample() {
         var pointList = new (int x, int y)[]
                               {
                                     ( 0, 0 ),
                                     ( 1, 1 ),
                                     ( 3, 5 ),
                                     ( 8, 6 ),
                                     ( 9, 9 ),
                               };
         var expectedLineSegments = new[]
                                          {
                                                ( ( 0, 0 ), ( 1, 1 ) ),
                                                ( ( 1, 1 ), ( 3, 5 ) ),
                                                ( ( 3, 5 ), ( 8, 6 ) ),
                                                ( ( 8, 6 ), ( 9, 9 ) ),
                                          };

         var actualLineSegments = pointList.Chain()
                                           .ToArray();

         CollectionAssert.AreEqual(expectedLineSegments, actualLineSegments);
      }


      [TestMethod]
      public void Chain_from0Element() {
         CollectionAssert.AreEqual(Array.Empty<int>(), EnumerableUtil.Chain(Enumerable.Empty<int>())
                                                                     .ToArray());
      }


      [TestMethod]
      public void Chain_from1Element() {
         CollectionAssert.AreEqual(Array.Empty<int>(), EnumerableUtil.Chain(new int[] { 42 })
                                                                     .ToArray());
      }


      [TestMethod]
      public void Chain_from2Elements() {
         CollectionAssert.AreEqual(new (int, int)[] { ( 123, 789 ) },
                                   EnumerableUtil.Chain(new int[] { 123, 789 })
                                                 .ToArray());
      }


      [TestMethod]
      public void Chain_from3Elements() {
         CollectionAssert.AreEqual(new (int, int)[]
                                         {
                                               ( 123, 456 ),
                                               ( 456, 789 ),
                                         },
                                   EnumerableUtil.Chain(new int[] { 123, 456, 789 })
                                                 .ToArray());
      }


      [TestMethod]
      public void Chain_from4Elements() {
         CollectionAssert.AreEqual(new (int, int)[]
                                         {
                                               ( 12, 34 ),
                                               ( 34, 56 ),
                                               ( 56, 78 ),
                                         },
                                   EnumerableUtil.Chain(new int[] { 12, 34, 56, 78 })
                                                 .ToArray());
      }
      #endregion Chain


      #region ConcatParts

      [TestMethod]
      public void ConcatParts_sample() {
         string[] expected = new[] { "<", "a", "b", "c", ">" };
         IEnumerable<string> sequence = new[] { "a", "b", "c" };
         string[] actual = EnumerableUtil.ConcatParts<string>(new object[] { "<", sequence, ">" })
                                         .ToArray();
         CollectionAssert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ConcatParts_sample_paramArray() {
         string[] expected = new[] { "<", "a", "b", "c", ">" };
         IEnumerable<string> sequence = new[] { "a", "b", "c" };
         string[] actual = EnumerableUtil.ConcatParts<string>("<", sequence, ">")
                                         .ToArray();
         CollectionAssert.AreEqual(expected, actual);
      }
      #endregion ConcatParts


      #region Repeat

      [TestMethod]
      public void Repeat_sample() {
         string[] expected = new[] { "abc", "abc", "abc", "abc" };
         string[] actual = EnumerableUtil.Repeat("abc", 4).ToArray();
         CollectionAssert.AreEqual(expected, actual);
      }
      #endregion Repeat


      #region ReplaceAll

      [TestMethod]
      public void ReplaceAll_valid_sample() {
         testReplaceAll(new[] { "We", "have", "what", "We", "need" },
                        new[] { "I", "have", "what", "I", "need" },
                        s => s == "I", "We");
      }


      [TestMethod]
      public void ReplaceAll_valid_empty() {
         testReplaceAll(Array.Empty<object>(), Array.Empty<object>(), x => false, null);
      }


      [TestMethod]
      public void ReplaceAll_valid_falsePredicate() {
         testReplaceAll(new int[] { 1, 3, 5, 7, 9 }, new int[] { 1, 3, 5, 7, 9 }, x => false, -99);
      }


      [TestMethod]
      public void ReplaceAll_valid_truePredicate() {
         testReplaceAll(new int[] { -99, -99, -99, -99, -99 }, new int[] { 1, 3, 5, 7, 9 }, x => true, -99);
      }


      private static void testReplaceAll<T>(T[] expected, T[] source, Func<T, bool> predicateFunc, T replacement) {
         var actual = source.ReplaceAll(predicateFunc, replacement);
         CollectionAssert.AreEqual(expected, actual.ToArray());
      }
      #endregion ReplaceAll


      [TestMethod]
      public void ZipToEnd_WithDefaults_sample() {
         int[] seq1 = { 1, 2, 3 };
         int[] seq2 = { 11, 22, 33, 44, 55 };

         var expected = new (int element1, int element2, bool isSequence1Ended, bool isSequence2Ended)[]
                           {
                              ( 1, 11, false, false ),
                              ( 2, 22, false, false ),
                              ( 3, 33, false, false ),
                              ( default( int ), 44, true, false ),
                              ( default( int ), 55, true, false ),
                           };
         var actual = seq1.ZipToEnd(seq2)
                          .ToArray();

         CollectionAssert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ZipToEnd_WithResultFuncSpecified_sample() {
         int[] seq1 = { 1, 2, 3 };
         int[] seq2 = { 11, 22, 33, 44, 55 };

         var expected = new[]
                           {
                              new object[] { false, false, 1             , 11 },
                              new object[] { false, false, 2             , 22 },
                              new object[] { false, false, 3             , 33 },
                              new object[] { true , false, default( int ), 44 },
                              new object[] { true , false, default( int ), 55 },
                           };
         var actual = seq1.ZipToEnd(seq2, selectResult)
                          .ToArray();

         CollectionAssert.AreEqual(expected, actual, new TestArrayComparer());

         object[] selectResult((int element1, int element2, bool isSequence1Ended, bool isSequence2Ended) tuple)
            => new object[] { tuple.isSequence1Ended, tuple.isSequence2Ended, tuple.element1, tuple.element2 };
      }


      [TestMethod]
      public void ZipToEnd_WithDefaultsSpecified_sample() {
         int[] seq1 = { 1, 2, 3 };
         int[] seq2 = { 11, 22, 33, 44, 55 };

         var expected = new (int element1, int element2, bool isSequence1Ended, bool isSequence2Ended)[]
                           {
                              ( 1  , 11, false, false ),
                              ( 2  , 22, false, false ),
                              ( 3  , 33, false, false ),
                              ( -99, 44, true , false ),
                              ( -99, 55, true , false ),
                           };
         var actual = seq1.ZipToEnd(seq2, default1: -99, default2: -22)
                          .ToArray();

         CollectionAssert.AreEqual(expected, actual);
      }



      private class TestArrayComparer : IComparer {
         public int Compare(object x, object y)
            => equals(x, y) ? 0 : -1;

         private static bool equals(object x, object y)
            => ( x is object[] xArray && y is object[] yArray )
            && xArray.SequenceEqual(yArray);
      }

   }
}
