using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.StaticUtilities;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_TupleUtil {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void ToTuple_invalid_null() {
         TupleUtil.ToTuple(null);
      }


      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void ToTuple_invalid_array1() {
         TupleUtil.ToTuple(new string[] { "1" });
      }


      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void ToTuple_invalid_array3() {
         TupleUtil.ToTuple(new string[] { "1", "2", "3" });
      }


      [TestMethod]
      public void ToTuple_valid_typical() {
         (string, string) tuple = TupleUtil.ToTuple(new string[] { "1", "2" });
         Assert.IsNotNull(tuple);
         Assert.AreEqual("1", tuple.Item1);
         Assert.AreEqual("2", tuple.Item2);
      }


      [TestMethod]
      public void ToTuple_valid_null() {
         (string, string) tuple = TupleUtil.ToTuple(new string[] { null, null });
         Assert.IsNotNull(tuple);
         Assert.AreEqual(null, tuple.Item1);
         Assert.AreEqual(null, tuple.Item2);
      }


      [TestMethod]
      public void ToTuple_valid_empty() {
         (string, string) tuple = TupleUtil.ToTuple(new string[] { "", "" });
         Assert.IsNotNull(tuple);
         Assert.AreEqual("", tuple.Item1);
         Assert.AreEqual("", tuple.Item2);
      }
   }
}
