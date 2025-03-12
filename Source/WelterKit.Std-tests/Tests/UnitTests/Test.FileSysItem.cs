using System;
using WelterKit.Std;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_FileSysItem {
      [TestMethod]
      public void TypeChar_1() {
         Assert.AreEqual("F", FileSysFileItem.CreateForTest("base", "filename", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L).TypeChar);
         Assert.AreEqual("d", FileSysDirItem.CreateForTest("base", "dirname", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)).TypeChar);
      }


      [TestMethod]
      public void RelativePath_1() {
         Assert.AreEqual("subdir\\filename", FileSysFileItem.CreateForTest("base", "subdir\\filename", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L).RelativePath);
         Assert.AreEqual("subdir\\dirname", FileSysDirItem.CreateForTest("base", "subdir\\dirname", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)).RelativePath);
      }


      [TestMethod]
      public void Name_1() {
         Assert.AreEqual("filename", FileSysFileItem.CreateForTest("base", "subdir\\filename", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L).Name);
         Assert.AreEqual("dirname", FileSysDirItem.CreateForTest("base", "subdir\\dirname", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)).Name);
      }


      [TestMethod]
      public void FullName() {
         Assert.AreEqual("base\\subdir\\filename", FileSysFileItem.CreateForTest("base", "subdir\\filename", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc), 123L).FullName);
         Assert.AreEqual("base\\subdir\\dirname", FileSysDirItem.CreateForTest("base", "subdir\\dirname", new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)).FullName);
      }


      //[TestMethod]
      //public void FromFileSysInfo() {
      //}
   }
}
