using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.StaticUtilities;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_MathUtil_Double {
      [TestMethod]
      public void ScaleRange_samples() {
         Assert.AreEqual(138d, MathUtil.ScaleRange(19d,
                                                   fromRange: ( 10d, 20d ),
                                                   toRange: ( 120d, 140d )));
         Assert.AreEqual(120d, MathUtil.ScaleRange(0.2d,
                                                   fromRange: ( 0d, 1d ),
                                                   toRange: ( 110d, 160d )));
         Assert.AreEqual(21.11111111111111d, MathUtil.ScaleRange(70d,
                                                                 fromRange: ( 32d, 212d ),
                                                                 toRange: ( 0d, 100d )));
      }


      [TestMethod]
      public void ScaleRange_zeros() {
         Assert.AreEqual(0d, MathUtil.ScaleRange(0d,
                                                 fromRange: ( 0d, 0d ),
                                                 toRange: ( 0d, 0d )));
      }


      [TestMethod]
      public void ScaleRange_outOfRange() {
         Assert.AreEqual(140d, MathUtil.ScaleRange(20d,
                                                   fromRange: ( 0d, 10d ),
                                                   toRange: ( 100d, 120d )));
      }


      [TestMethod]
      public void IsApproximatelyEqualTo_sample() {
         Assert.IsTrue(MathUtil.IsApproximatelyEqualTo(0.19999999999999999999d, 0.2d));
         Assert.IsFalse(MathUtil.IsApproximatelyEqualTo(10.123d, 10.125d, 0.001d));
      }
   }
}
