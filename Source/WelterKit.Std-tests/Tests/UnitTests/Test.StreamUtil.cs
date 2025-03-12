using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_StreamUtil {
      [TestMethod]
      public void EnumBytes_valid_empty() {
         testEnumBytes(new byte[] { });
      }


      [TestMethod]
      public void EnumBytes_valid_multi() {
         testEnumBytes(new byte[] { 0, 1, 2, 3, 4, 5 });
      }


      private static void testEnumBytes(byte[] bytes_expected) {
         byte[] bytes_actual;
         using ( var stream = new MemoryStream(bytes_expected) )
            bytes_actual = StreamUtil.EnumBytes(stream).ToArray();

         CollectionAssert.AreEqual(bytes_expected, bytes_actual);
      }
   }
}
