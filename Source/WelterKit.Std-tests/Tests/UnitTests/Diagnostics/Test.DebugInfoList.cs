using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Diagnostics;



namespace WelterKit_Tests.Tests.UnitTests.Diagnostics {
   [TestClass]
   public class Test_DebugInfoList {
      [TestMethod]
      public void ObjectListInfo_1() {
         int[] list = new int[]
                            {
                                  int.MinValue,
                                  -1,
                                  0,
                                  1,
                                  int.MaxValue,
                            };
         string expected = @"[cnt: 5] (Int32[]) System.Int32[]
000: (Int32) -2147483648
001: (Int32) -1
002: (Int32) 0
003: (Int32) 1
004: (Int32) 2147483647";
         string actual = new ObjectListInfo<int>(list).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ObjectListInfo_2() {
         int[] list = new int[]
                            {
                                  int.MinValue,
                                  -1,
                                  0,
                                  1,
                                  int.MaxValue,
                            };
         string expected = @"[cnt: 5] (Int32[]) System.Int32[]
000: (Int32) -2,147,483,648
001: (Int32) -1
002: (Int32) 0
003: (Int32) 1
004: (Int32) 2,147,483,647";
         string actual = new ObjectListInfo<int>(list, i => i.ToString("N0")).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void InLineList_null() {
         string expected = "[null]";
         string actual = new InlineListDebugInfo<double>(null, d => d.ToString("0.0")).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void InLineList_empty() {
         string expected = "[cnt: 0] (Double[]) {  }";
         string actual = new InlineListDebugInfo<double>(new double[] { }, d => d.ToString("0.0")).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void InLineList_1() {
         string expected = "[cnt: 3] (Double[]) { 1.0, 2.2, 3.3 }";
         string actual = new InlineListDebugInfo<double>(new double[] { 1.01, 2.22, 3.33 }, d => d.ToString("0.0")).ToString();
         Assert.AreEqual(expected, actual);
      }



















      [TestMethod]
      public void Diag_ObjectListInfo_NoFunc() {
         string expected = @"[cnt: 3] (Int32[]) System.Int32[]
000: (Int32) 1
001: (Int32) 222
002: (Int32) 33333";
         string actual = DiagStr.ObjectListInfo(new int[] { 1, 222, 33333 });
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Diag_ObjectListInfo_WithFunc() {
         string expected = @"[cnt: 3] (Int32[]) System.Int32[]
000: (Int32) $1
001: (Int32) $222
002: (Int32) $33,333";
         string actual = DiagStr.ObjectListInfo(new int[] { 1, 222, 33333 },
                                                i => i.ToString("C0"));
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Diag_ObjectListLine_byte() {
         string expected = "[cnt: 4] (Byte[]) { 0, 111, 222, 33 }";
         string actual = Diag.ObjectListLine(new byte[] { 0, 111, 222, 33 }).ToString();
         Assert.AreEqual(expected, actual);
      }
   }
}
