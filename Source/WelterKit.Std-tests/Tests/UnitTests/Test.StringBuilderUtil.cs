using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_StringBuilderUtil {
      [TestMethod]
      public void AppendIf_string_sample() {
         Assert.AreEqual("aabb", new StringBuilder("aa").AppendIf(true , "bb").ToString());
         Assert.AreEqual(  "aa", new StringBuilder("aa").AppendIf(false, "bb").ToString());
      }


      [TestMethod]
      public void AppendIf_StringBuilder_sample() {
         Assert.AreEqual("aabb", new StringBuilder("aa").AppendIf(true , new StringBuilder("bb")).ToString());
         Assert.AreEqual(  "aa", new StringBuilder("aa").AppendIf(false, new StringBuilder("bb")).ToString());
      }


      [TestMethod]
      public void AppendLines_IEnumerable_sample() {
         Assert.AreEqual("aabb\r\ncc\r\n", new StringBuilder("aa").AppendLines(new[] { "bb", "cc" }).ToString());
      }


      [TestMethod]
      public void AppendLines_params_sample() {
         Assert.AreEqual("aabb\r\ncc\r\n", new StringBuilder("aa").AppendLines("bb", "cc").ToString());
      }
   }
}
