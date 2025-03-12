using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.StaticUtilities;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_MathUtil_Float {
      [TestMethod]
      public void ScaleRange_samples() {
         Assert.AreEqual(138f, MathUtil.ScaleRange(19f,
                                                   fromRange: ( 10f, 20f ),
                                                   toRange: ( 120f, 140f )));
         Assert.AreEqual(120f, MathUtil.ScaleRange(0.2f,
                                                   fromRange: ( 0f, 1f ),
                                                   toRange: ( 110f, 160f )));
         Assert.AreEqual(21.111113f, MathUtil.ScaleRange(70f,
                                                         fromRange: ( 32f, 212f ),
                                                         toRange: ( 0f, 100f )));
      }


      [TestMethod]
      public void ScaleRange_zeros() {
         Assert.AreEqual(0f, MathUtil.ScaleRange(0f,
                                                 fromRange: ( 0f, 0f ),
                                                 toRange: ( 0f, 0f )));
      }


      [TestMethod]
      public void ScaleRange_outOfRange() {
         Assert.AreEqual(140f, MathUtil.ScaleRange(20f,
                                                   fromRange: ( 0f, 10f ),
                                                   toRange: ( 100f, 120f )));
      }


      [TestMethod]
      public void IsApproximatelyEqualTo_sample() {
         Assert.IsTrue(MathUtil.IsApproximatelyEqualTo(0.199999999999999f, 0.2f));
         Assert.IsFalse(MathUtil.IsApproximatelyEqualTo(10.123f, 10.125f, 0.001f));
      }
   }
}
