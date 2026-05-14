using System;
using System.Linq;
using WelterKit.Testing;


namespace WelterKit.FunctionalContainers_tests.Support;

[TestClass]
public class SequenceGenerator_Tests {
   [TestMethod]
   public void Test_RandomSequenceGenerator() {
      ISequenceGenerator seq = new RandomIntSequenceGenerator(4242);

      int[] expectedSequence =
         [
            1834865992, 1071811615, 1879216329, 2111132622, 877335630, 1360174786, 1208041663, 1261463278, 1683182456, 1107802323,
            1079767260, 554031781, 1718095855, 842595438, 564370835, 532595417, 54858973, 69776995, 1094701343, 1798953383
         ];
      Assert.AreEqual(expectedSequence[0], seq.Next());
      Assert.AreEqual(expectedSequence[1], seq.Next());
      Assert.AreEqual(expectedSequence[2], seq.Next());
      // test the next 17
      SequenceAssert.AreEqual(expectedSequence[3..], seq.EnumerateEndless().Take(17).ToArray(), null);
   }


   [TestMethod]
   public void Test_RecursiveSequenceGenerator() {
      static int add12(int n) => n + 12;
      ISequenceGenerator seq = new RecursiveSequenceGenerator(-1, add12);

      int[] expectedSequence = [-1, 11, 23, 35, 47, 59, 71, 83, 95, 107, 119, 131, 143];

      Assert.AreEqual(expectedSequence[0], seq.Next());
      Assert.AreEqual(expectedSequence[1], seq.Next());
      Assert.AreEqual(expectedSequence[2], seq.Next());
      // test the next 10
      SequenceAssert.AreEqual(expectedSequence[3..], seq.EnumerateEndless().Take(10).ToArray(), null);
   }
}
