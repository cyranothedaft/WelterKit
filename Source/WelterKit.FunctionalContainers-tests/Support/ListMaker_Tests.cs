using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std.StaticUtilities;
using WelterKit.Testing;


namespace WelterKit.FunctionalContainers_tests.Support;

[TestClass]
public class ListMaker_Tests {
   [TestMethod]
   public void MakeListOfSize_Sample() {
      int[] possibleElements = [2, 3, 5, 7, 11, 13, 17, 19];

      ISequenceGenerator gen = new RandomIntSequenceGenerator(42 * 42); // known seed to generate a known sequence

      SequenceAssert.AreEqual(new int[] { }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 0), null);
      SequenceAssert.AreEqual(new int[] { 2 }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 1), null);
      SequenceAssert.AreEqual(new int[] { 17 }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 1), null);
      SequenceAssert.AreEqual(new int[] { 7, 11, 13, 19, 19 }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 5), null);
   }


   [TestMethod]
   public void ChoosePermutations_Sample() {
      int[] possibleElements = [2, 3, 5, 7, 11, 13, 17, 19];
      ISequenceGenerator gen = new RandomIntSequenceGenerator(42 * 42); // known seed to generate a known sequence

      int[][] permutations = ListMaker.ChoosePermutations(possibleElements, gen, count: 12, minLength: 1, maxLength: 10)
                                       // TODO: distinct?
                                      .ToArray();

      int[][] expected =
         [
               [17, 7, 5, 13, 13, 5, 11, 5, 5],
               [3],
               [2, 11],
               [13, 3, 3, 3, 5],
               [7, 5, 5, 19, 13, 3, 5, 2, 5, 5],
               [3, 5, 2, 19, 2, 2, 17, 7, 7, 13],
               [13, 7, 19, 2, 11, 19],
               [17, 17],
               [19],
               [17, 11, 13, 13, 17, 2],
               [19, 5, 19, 3, 3],
               [2, 13, 5, 2, 5]
         ];

      SequenceAssert.AreEqual(expected, permutations, assertAreEqualAction: (exp, act, msg)
            => SequenceAssert.AreEqual(exp, act, assertAreEqualAction: null, msg));
   }


   private static void write(IEnumerable<int> list)
      => Console.WriteLine(list.JoinString(", ", x => x.ToString()));
}
