using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_TextWriterUtil {
      [TestMethod]
      public void GetString_sample() {
         Assert.AreEqual("written string - line 1\r\n" +
                         "written string - line 2\r\n", TextWriterUtil.GetString(writerAction));

         void writerAction(TextWriter writer) {
            writer.WriteLine("written string - line 1");
            writer.WriteLine("written string - line 2");
         }
      }
   }
}
