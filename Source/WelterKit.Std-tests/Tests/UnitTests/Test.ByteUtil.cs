using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_ByteUtil {
      [TestMethod]
      public void HexToByteArray_empty() {
         Assert.IsNull(ByteUtil.HexToByteArray(null));
         CollectionAssert.AreEqual(new byte[] { }, ByteUtil.HexToByteArray(""));
      }


      [TestMethod]
      public void HexToByteArray_invalid() {
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("0"));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("a"));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("g"));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray(" "));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("."));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("000"));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("123"));
         Assert.ThrowsException<ArgumentOutOfRangeException>(() => ByteUtil.HexToByteArray("abcdefg"));
      }


      [TestMethod]
      public void HexToByteArray_valid() {
         CollectionAssert.AreEqual(new byte[] { 0 }, ByteUtil.HexToByteArray("00"));
         CollectionAssert.AreEqual(new byte[] { 0, 0 }, ByteUtil.HexToByteArray("0000"));
         CollectionAssert.AreEqual(new byte[] { 0xff, 0, 0xff, 0, 0xff }, ByteUtil.HexToByteArray("ff00ff00ff"));
      }


      [TestMethod]
      public void HexToByteArray_valid_uppercase() {
         CollectionAssert.AreEqual(new byte[] { 0xab, 0xcd, 0xef }, ByteUtil.HexToByteArray("ABCDEF"));
         CollectionAssert.AreEqual(new byte[] { 0xff, 0, 0xff, 0, 0xff }, ByteUtil.HexToByteArray("FF00FF00FF"));
      }


      [TestMethod]
      public void ByteArrayToHex_empty() {
         Assert.IsNull(ByteUtil.ByteArrayToHex(null));
         Assert.AreEqual("", ByteUtil.ByteArrayToHex(new byte[] { }));
      }


      [TestMethod]
      public void ByteArrayToHex_valid() {
         Assert.AreEqual("00", ByteUtil.ByteArrayToHex(new byte[] { 0 }));
         Assert.AreEqual("0000", ByteUtil.ByteArrayToHex(new byte[] { 0, 0 }));
         Assert.AreEqual("ff00ff00ff", ByteUtil.ByteArrayToHex(new byte[] { 0xff, 0, 0xff, 0, 0xff }));
      }


      [TestMethod]
      public void CompareByteArrays_invalid() {
         Assert.ThrowsException<ArgumentNullException>(() => ByteUtil.CompareByteArrays(null, null));
         Assert.ThrowsException<ArgumentNullException>(() => ByteUtil.CompareByteArrays(new byte[] { 0 }, null));
         Assert.ThrowsException<ArgumentNullException>(() => ByteUtil.CompareByteArrays(null, new byte[] { 0 }));
      }


      [TestMethod]
      public void CompareByteArrays_valid_equal() {
         Assert.AreEqual(0, ByteUtil.CompareByteArrays(new byte[] { }, new byte[] { }));
         Assert.AreEqual(0, ByteUtil.CompareByteArrays(new byte[] { 0 }, new byte[] { 0 }));
         Assert.AreEqual(0, ByteUtil.CompareByteArrays(new byte[] { 0, 0 }, new byte[] { 0, 0 }));
         Assert.AreEqual(0, ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
      }


      [TestMethod]
      public void CompareByteArrays_valid_unequal_lendiff() {
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { }, new byte[] { 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0 }, new byte[] { }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { }, new byte[] { 0, 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0, 0 }, new byte[] { }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0 }, new byte[] { 0, 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0, 0 }, new byte[] { 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff, 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff, 0, 0xff }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff, 0, 0xff, 0 }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0xff }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0xff, 0 }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0 }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
      }


      [TestMethod]
      public void CompareByteArrays_valid_unequal_lensame() {
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0 }, new byte[] { 1 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 1 }, new byte[] { 0 }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0, 0, 0 }, new byte[] { 0, 0, 1 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0, 0, 1 }, new byte[] { 0, 0, 0 }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0, 0, 0, 0, 0 }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0, 0, 0, 0, 0 }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
         Assert.IsTrue(0 < ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0xff }, new byte[] { 0xff, 0, 0xff, 0, 0 }));
         Assert.IsTrue(0 > ByteUtil.CompareByteArrays(new byte[] { 0xff, 0, 0xff, 0, 0 }, new byte[] { 0xff, 0, 0xff, 0, 0xff }));
      }
   }
}
