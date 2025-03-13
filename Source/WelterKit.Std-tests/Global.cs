using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests {
   [TestClass]
   public class Global {
      internal const string RepositoryRoot = @"..\..\..\..\..";
      public static readonly string TestDataSourceDir = Path.Combine(RepositoryRoot, @"testdata");


      [AssemblyInitialize]
      public static void TestAssemblyInit(TestContext testContext) {
//         Test_WardStampIO.AssemblyInit(testContext);
//         Test_FileStatDbIO.AssemblyInit(testContext);
//         Test_WardDefinitionIO.AssemblyInit(testContext);
//         Test_LocalWardLocationIO.AssemblyInit(testContext);
      }


      [TestMethod]
      public void dummytest() { }
   }
}
