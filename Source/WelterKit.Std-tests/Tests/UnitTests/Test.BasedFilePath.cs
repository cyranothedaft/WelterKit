using System;
using WelterKit.Std;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Diagnostics;



namespace WelterKit.Std_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_BasedFilePath {
      [TestMethod]
      public void Base() {
         Assert.AreEqual(@"base", new BasedFilePath("base", "tail").Base);
         Assert.AreEqual(@"\base", new BasedFilePath(@"\base", "tail").Base);
         Assert.AreEqual(@"base\", new BasedFilePath(@"base\", "tail").Base);
         Assert.AreEqual(@"\base\", new BasedFilePath(@"\base\", "tail").Base);
      }


      [TestMethod]
      public void Tail() {
         Assert.AreEqual("tail", new BasedFilePath("base", "tail").Tail);
         Assert.AreEqual("tail", new BasedFilePath("base", @"\tail").Tail);
         Assert.AreEqual("tail", new BasedFilePath("base", @"tail\").Tail);
         Assert.AreEqual("tail", new BasedFilePath("base", @"\tail\").Tail);
      }


      [TestMethod]
      public void ToString_1() {
         Assert.AreEqual(@"1\2", new BasedFilePath("1", "2").ToString());
         Assert.AreEqual(@"abc\xyz", new BasedFilePath("abc", "xyz").ToString());
         Assert.AreEqual(@"a\b\c\x\y\z", new BasedFilePath(@"a\b\c", @"x\y\z").ToString());
         Assert.AreEqual(@"C:\1", new BasedFilePath(@"C:", "1").ToString());
         Assert.AreEqual(@"C:\1", new BasedFilePath(@"C:\", "1").ToString());
         Assert.AreEqual(@"C:\1\2", new BasedFilePath(@"C:\1", "2").ToString());
         Assert.AreEqual(@"C:\1\2", new BasedFilePath(@"C:", @"1\2").ToString());
         Assert.AreEqual(@"C:\1\2", new BasedFilePath(@"C:\", @"1\2").ToString());
      }


      [TestMethod]
      public void ToString_blanks() {
         Assert.AreEqual(@"", new BasedFilePath("", "").ToString());
         Assert.AreEqual(@"", new BasedFilePath("", null).ToString());
         Assert.AreEqual(@"", new BasedFilePath(null, "").ToString());
         Assert.AreEqual(@"", new BasedFilePath(null, null).ToString());
         Assert.AreEqual(@"xyz", new BasedFilePath("", "xyz").ToString());
         Assert.AreEqual(@"xyz", new BasedFilePath(null, "xyz").ToString());
         Assert.AreEqual(@"abc", new BasedFilePath("abc", "").ToString());
         Assert.AreEqual(@"abc", new BasedFilePath("abc", null).ToString());
      }


      [TestMethod]
      public void ToString_extraSlashes() {
         Assert.AreEqual(@"\abc\xyz", new BasedFilePath(@"\abc\", @"xyz\").ToString());
         Assert.AreEqual(@"C:\abc", new BasedFilePath(@"C:\", @"abc").ToString());
      }


      [TestMethod]
      public void ToString_absolutePaths() {
         Assert.AreEqual(@"\abc\xyz", new BasedFilePath(@"\abc\", @"\xyz\").ToString());
         Assert.AreEqual(@"\abc\xyz", new BasedFilePath(@"\abc", @"\xyz\").ToString());
         Assert.AreEqual(@"C:\xyz", new BasedFilePath(@"\abc\", @"C:\xyz\").ToString());
         Assert.AreEqual(@"C:\xyz", new BasedFilePath(@"C:\abc", @"C:\xyz").ToString());
         Assert.AreEqual(@"C:\abc", new BasedFilePath(@"C:\", @"\abc").ToString());
         Assert.AreEqual(@"C:\abc", new BasedFilePath(@"C:", @"\abc").ToString());
      }


      [TestMethod]
      public void ToStringWithNewBase_1() {
         Assert.AreEqual(@"newbase\tail", new BasedFilePath("base", "tail").ToStringWithNewBase("newbase"));
      }


      [TestMethod]
      public void GetParentDirectory() {
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath("base", "sub1"), new BasedFilePath("base", @"sub1\sub2").GetParentDirectory()));
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath("base", ""), new BasedFilePath("base", "tail").GetParentDirectory()));
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath("base", ""), new BasedFilePath("base", "").GetParentDirectory()));
      }


      [TestMethod]
      public void ReplaceBase() {
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath("newbase", "path"), new BasedFilePath(@"\full", "path").ReplaceBase("newbase")));
      }


      [TestMethod]
      public void FromBaseRoot_1() {
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath("root", ""), BasedFilePath.FromBaseRoot("root")));
      }


      [TestMethod]
      public void FromFullAndBase() {
         Assert.IsTrue(BasedFilePath.Comparer.Equals(new BasedFilePath(@"\full", "path"), BasedFilePath.FromFullAndBase(@"\full\path", @"\full")));
      }


      [TestMethod]
      public void FromFullAndTail() {
         BasedFilePath expected = new BasedFilePath(@"\full\", "path"),
                       actual = BasedFilePath.FromFullAndTail(@"\full\path", "path");
         Assert.IsTrue(BasedFilePath.Comparer.Equals(expected, actual), $"{DiagStr.InfoShort(expected)} =?= {DiagStr.InfoShort(actual)}");
      }
   }
}
