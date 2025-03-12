using System;
using WelterKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_FileSysItem_Comparator {
      [TestMethod]
      public void Compare_equal() {
         Assert.AreEqual(0, FileSysFileItem.Comparer.Compare(FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L),
                                                             FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L)));
         Assert.AreEqual(0, FileSysDirItem.Comparer.Compare(FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                                                            FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc))));
      }


      [TestMethod]
      public void Compare_equal_diffCase() {
         Assert.AreEqual(0, FileSysFileItem.Comparer.Compare(FileSysFileItem.CreateForTest("BASE", "RELATIVEPATH", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L),
                                                             FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L)));
         Assert.AreEqual(0, FileSysDirItem.Comparer.Compare(FileSysDirItem.CreateForTest("BASE", "RELATIVEPATH", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                                                            FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc))));
      }


      [TestMethod]
      public void Equals_equal() {
         Assert.IsTrue(FileSysFileItem.Comparer.Equals(FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L),
                                                       FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L)));
         Assert.IsTrue(FileSysDirItem.Comparer.Equals(FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                                                      FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc))));
      }


      [TestMethod]
      public void Equals_equal_diffCase() {
         Assert.IsTrue(FileSysFileItem.Comparer.Equals(FileSysFileItem.CreateForTest("BASE", "RELATIVEPATH", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L),
                                                       FileSysFileItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L)));
         Assert.IsTrue(FileSysDirItem.Comparer.Equals(FileSysDirItem.CreateForTest("BASE", "RELATIVEPATH", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                                                      FileSysDirItem.CreateForTest("base", "relativePath", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc))));
      }
   }
}
