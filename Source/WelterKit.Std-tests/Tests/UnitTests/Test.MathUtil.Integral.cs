using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;


namespace WelterKit.Std_Tests.Tests.UnitTests;

[TestClass]
[TestCategory("Unit")]
public class Test_MathUtil_Integral {
   [TestMethod]
   public void ReduceToRange_Samples() {
      Assert.AreEqual( 0,    0.ReduceToRange(  0,   0));
      Assert.AreEqual(42,   42.ReduceToRange( 42,  42));
      Assert.AreEqual( 4,    4.ReduceToRange(  0,   5));
      Assert.AreEqual( 0,    6.ReduceToRange(  0,   5));
      Assert.AreEqual( 1,    7.ReduceToRange(  0,   5));
      Assert.AreEqual( 2,   26.ReduceToRange(  0,   5));
      Assert.AreEqual(15,  115.ReduceToRange(  0,  99));
      Assert.AreEqual(115, 115.ReduceToRange(100, 199));
      Assert.AreEqual(115, 215.ReduceToRange(100, 199));
   }
}
