using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.StaticUtilities;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_FileSystemUtil {
      [TestMethod]
      public void GetPathRelativeTo_1() {
         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a\"));
      }


      [TestMethod]
      public void GetPathRelativeTo_Valid_1() {
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a\b\"));
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a\b"));
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"\a\b\c", @"\a\b"));
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"a\b\c", @"a\b"));
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"\b\c", @"\b"));
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"b\c", @"b"));

         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a\"));
         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a"));
         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"\a\b\c", @"\a"));
         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"a\b\c", @"a"));
      }


      [TestMethod]
      public void GetPathRelativeTo_Valid_2() {
         Assert.AreEqual(@"b\c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\a\"));
      }


      [TestMethod]
      public void GetPathRelativeTo_Valid_Case() {
         Assert.AreEqual(@"c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"C:\A\B\"));
      }


      [TestMethod]
      public void GetPathRelativeTo_Invalid_1() {
         Assert.AreEqual(null, FileSystemUtil.GetPathRelativeTo(( string )null, null));
         Assert.AreEqual(null, FileSystemUtil.GetPathRelativeTo(( string )null, ""));
         Assert.AreEqual(null, FileSystemUtil.GetPathRelativeTo("", null));
         Assert.AreEqual("", FileSystemUtil.GetPathRelativeTo("", ""));

         Assert.AreEqual(@"C:\a\b\c", FileSystemUtil.GetPathRelativeTo(@"C:\a\b\c", @"asdf"));
      }


      [TestMethod]
      public void IsFailure() {
         Assert.IsFalse(FileSystemUtil.IsFailure(FileSystemUtil.TryPurgeDirResult.None));
         Assert.IsFalse(FileSystemUtil.IsFailure(FileSystemUtil.TryPurgeDirResult.DirNoLongerExists));
         Assert.IsFalse(FileSystemUtil.IsFailure(FileSystemUtil.TryPurgeDirResult.PurgedAndConfirmed));

         Assert.IsTrue(FileSystemUtil.IsFailure(FileSystemUtil.TryPurgeDirResult.NoErrorButDirRemains));
         Assert.IsTrue(FileSystemUtil.IsFailure(FileSystemUtil.TryPurgeDirResult.DirectoryNotEmptyException));
      }


      [TestMethod]
      public void IsValidFilePathName_Blank() {
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName(null));
         Assert.IsTrue(FileSystemUtil.IsValidFilePathName(""));
         Assert.IsTrue(FileSystemUtil.IsValidFilePathName(" "));
      }


      [TestMethod]
      public void IsValidFilePathName_Valid() {
         Assert.IsTrue(FileSystemUtil.IsValidFilePathName("abc"));
         Assert.IsTrue(FileSystemUtil.IsValidFilePathName("123"));
      }


      [TestMethod]
      public void IsValidFilePathName_Invalid() {
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("/"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("\\"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName(":"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("*"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("?"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("\""));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("<"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName(">"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("|"));

         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("\0"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("\n"));
         Assert.IsFalse(FileSystemUtil.IsValidFilePathName("\r"));
      }


      [TestMethod]
      public void GetDirectoryDepth_0() {
         Assert.AreEqual(0, FileSystemUtil.GetDirectoryDepth(null));
         Assert.AreEqual(0, FileSystemUtil.GetDirectoryDepth(""));
         Assert.AreEqual(0, FileSystemUtil.GetDirectoryDepth("\\"));
      }


      [TestMethod]
      public void GetDirectoryDepth_1plus() {
         testFor(1, "abc");
         testFor(2, "abc", "def");
         testFor(3, "abc", "def", "xyz");

         void testFor(int expected, params string[] parts) {
            const string slash = "\\";
            string middle = string.Join(slash, parts),
                   leading = slash + middle,
                   trailing = middle + slash,
                   both = slash + middle + slash;
            foreach ( string path in new[] { middle, leading, trailing, both } )
               Assert.AreEqual(expected, FileSystemUtil.GetDirectoryDepth(path), path);
         }
      }


      [TestMethod]
      public void GetDirectoryDepth_otherSlash() {
         Assert.AreEqual(1, FileSystemUtil.GetDirectoryDepth("/"));
         Assert.AreEqual(1, FileSystemUtil.GetDirectoryDepth("/asdf"));
         Assert.AreEqual(1, FileSystemUtil.GetDirectoryDepth("asdf/"));
         Assert.AreEqual(1, FileSystemUtil.GetDirectoryDepth("/asdf/"));
      }
   }
}
