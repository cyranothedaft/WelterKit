using System;
using System.Linq;
using WelterKit.FunctionalContainers.Containers;



namespace WelterKit.FunctionalContainers_tests.Support;

[TestClass]
public class SequenceGenerator_Tests {
   [TestMethod]
   public void Test_RandomSequenceGenerator() {
      ISequenceGenerator seq = new RandomIntSequenceGenerator(4242.AsSome());
      // ISequenceGenerator seq = new RandomIntSequenceGenerator(0, 10, 4242);

      int[] expectedSequence = [8, 4, 8, 9, 4, 6, 5, 5, 7, 5, 5, 2, 8, 3, 2, 2, 0, 0, 5, 8];

      Assert.AreEqual(expectedSequence[0], seq.Next());
      Assert.AreEqual(expectedSequence[1], seq.Next());
      Assert.AreEqual(expectedSequence[2], seq.Next());
      // test the next 17
      CollectionAssert.AreEqual(expectedSequence[3..], seq.EnumerateEndless().Take(17).ToArray());
   }


   [TestMethod]
   public void Test_RecursiveSequenceGenerator() {
      ISequenceGenerator seq = new RecursiveSequenceGenerator(0, i => i + 12);

      int[] expectedSequence = [0, 12, 24, 36, 48, 60, 72, 84, 96, 108, 120, 132, 144];

      Assert.AreEqual(expectedSequence[0], seq.Next());
      Assert.AreEqual(expectedSequence[1], seq.Next());
      Assert.AreEqual(expectedSequence[2], seq.Next());
      // test the next 10
      CollectionAssert.AreEqual(expectedSequence[3..], seq.EnumerateEndless().Take(10).ToArray());
   }
}
