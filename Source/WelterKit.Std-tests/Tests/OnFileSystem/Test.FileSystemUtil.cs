/* TODO
using System;
using System.IO;
using System.Linq;
using WelterKit.Std.Diagnostics;
using WelterKit.Testing;
using WelterKit.Std.StaticUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests.Tests.OnFileSystem {
   [TestClass]
   [TestCategory("Unit with filesystem")]
   public class Test_FileSystemUtil : TesterBase {
      protected override string UnresolvedTempDir => Path.Combine(base.UnresolvedTempDir, nameof( Test_FileSystemUtil ));


      [TestMethod]
      public void EnumFileSysItemsRecursively_none() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_none",
                                         expectedFileFunc: testLoc => new string[] { },
                                         recoverTestState: false);
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_none_includeRoot() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_none",
                                         includeRoot: true,
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}",
                                                       },
                                         recoverTestState: false);
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_1file() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_1file",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"F {testLoc.Dir}\file1.txt",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_1file_includeRoot() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_1file_includeRoot",
                                         includeRoot: true,
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}",
                                                          $@"F {testLoc.Dir}\file1.txt",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_3files() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_3files",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"F {testLoc.Dir}\file1.txt",
                                                          $@"F {testLoc.Dir}\file2.txt",
                                                          $@"F {testLoc.Dir}\file3.txt",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_1dir() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_1dir",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}\subdir1",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_1dir_includeRoot() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_1dir_includeRoot",
                                         includeRoot: true,
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}",
                                                          $@"d {testLoc.Dir}\subdir1",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_3dirs() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_3dirs",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}\subdir1",
                                                          $@"d {testLoc.Dir}\subdir2",
                                                          $@"d {testLoc.Dir}\subdir3",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_3dirsNested() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_3dirsNested",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}\subdir1",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2\subdir3",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_3dirsNestedWithFiles() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_3dirsNestedWithFiles",
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"F {testLoc.Dir}\file1.txt",
                                                          $@"d {testLoc.Dir}\subdir1",
                                                          $@"F {testLoc.Dir}\subdir1\file2.txt",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2",
                                                          $@"F {testLoc.Dir}\subdir1\subdir2\file3.txt",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2\subdir3",
                                                          $@"F {testLoc.Dir}\subdir1\subdir2\subdir3\file4.txt",
                                                       });
      }


      [TestMethod]
      public void EnumFileSysItemsRecursively_3dirsNestedWithFiles_includeRoot() {
         testEnumFileSysItemsRecursively("EnumFileSysItemsRecursively_3dirsNestedWithFiles_includeRoot",
                                         includeRoot: true,
                                         expectedFileFunc: testLoc => new[]
                                                       {
                                                          $@"d {testLoc.Dir}",
                                                          $@"F {testLoc.Dir}\file1.txt",
                                                          $@"d {testLoc.Dir}\subdir1",
                                                          $@"F {testLoc.Dir}\subdir1\file2.txt",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2",
                                                          $@"F {testLoc.Dir}\subdir1\subdir2\file3.txt",
                                                          $@"d {testLoc.Dir}\subdir1\subdir2\subdir3",
                                                          $@"F {testLoc.Dir}\subdir1\subdir2\subdir3\file4.txt",
                                                       });
      }


      private void testEnumFileSysItemsRecursively(string testName, bool includeRoot = false,
                                                   Func<FileSysTestLocation, string[]> expectedFileFunc = null,
                                                   bool recoverTestState = true) {
         if ( expectedFileFunc == null ) throw new ArgumentNullException(nameof( expectedFileFunc ));

         using ( var testLoc = new FileSysTestLocation(Path.Combine(TempDir, testName)) ) {
            if ( recoverTestState )
               TestUtil.RecoverTestState(testName, Global.TestDataSourceDir, testLoc.Dir, sourceIsZip: false);
            else
               Logger.L.Write("Not recovering state for test " + testName);

            var fileSysItems = FileSystemUtil.EnumFileSysItemsRecursively(testLoc.Dir, includeRoot);

            Assert.IsNotNull(fileSysItems);

            string[] actual = fileSysItems.Where(f => f.Name != TestUtil.MetadataFileName) // ignore \.metadata.json
                                          .OrderBy(f => f.FullName)
                                          .Select(f => f.TypeChar + " " + f.FullName)
                                          .ToArray();

            Logger.L.Write("<=> Test comparing file lists:");
            string[] expected = expectedFileFunc(testLoc);
            Logger.L.Write(DiagStr.Label("   expected", new StringListInfo(expected)));
            Logger.L.Write(DiagStr.Label("   actual  ", new StringListInfo(actual)));
            CollectionAssert.AreEqual(expected, actual, StringComparer.InvariantCultureIgnoreCase);
         }
      }
   }
}
*/