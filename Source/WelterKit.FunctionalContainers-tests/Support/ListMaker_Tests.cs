using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.Std.StaticUtilities;
using WelterKit.Testing;



namespace WelterKit.FunctionalContainers_tests.Support;

[TestClass]
public class ListMaker_Tests {
   [TestMethod]
   public void MakeListOfSize_Sample() {

      int[] possibleElements = [2, 3, 5, 7, 11, 13, 17, 19];

      //var r = newRng();
      //Console.WriteLine(string.Join(", ", Enumerable.Range(0, 20).Select(_ => r.Next(0, possibleElements.Length))));
      // ->  2, 3, 6, 0, 5, 0, 2, 3, 1, 6, 7, 5, 2, 1, 5, 5, 3, 5, 3, 2

      ISequenceGenerator gen = new RandomIntSequenceGenerator(0, possibleElements.Length,
                                                              42 * 42); // known seed to generate a known sequence

      CollectionAssert.AreEqual(new int[] { }   , ListMaker.MakeListOfSize(gen, possibleElements, listSize: 0));
      CollectionAssert.AreEqual(new int[] {  5 }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 1));
      CollectionAssert.AreEqual(new int[] {  7 }, ListMaker.MakeListOfSize(gen, possibleElements, listSize: 1));
   }


   [TestMethod]
   public void ChoosePermutations_Sample() {
      int[] possibleElements = [2, 3, 5, 7, 11, 13, 17, 19];
      ISequenceGenerator gen = new RandomIntSequenceGenerator(0, int.MaxValue, // possibleElements.Length,
                                                              42 * 42); // known seed to generate a known sequence
      // Console.WriteLine(gen.NextN(50).JoinString(", ", n => n.ToString()));
      // 2, 3, 6, 0, 5, 0, 2, 3, 1, 6, 7, 5, 2, 1, 5, 5, 3, 5, 3, 2, 1, 4, 0, 2, 4, 0, 4, 5, 6, 5, 3, 0, 5, 1, 4, 1, 1, 4, 3, 4, 5, 6, 3, 7, 7, 1, 3, 0, 3, 2

      int[][] alwaysTestThesePermutations =
         [
            [],
            possibleElements,
            possibleElements.RepeatSequence(2).ToArray()
         ];
      int[][] permutations = alwaysTestThesePermutations
                            .Concat(ListMaker.ChoosePermutations(possibleElements, gen, count: 12, minLength: 1, maxLength: 8))
                             // TODO: distinct?
                            .ToArray();

      // possible sizes: 

      int[][] expected =
         [
               [],
               [2, 3, 5, 7, 11, 13, 17, 19],
               [2, 3, 5, 7, 11, 13, 17, 19, 2, 3, 5, 7, 11, 13, 17, 19],
               [5],
               [7, 17],
               [13, 2, 5],
               [3, 17, 19, 13, 5, 3],
               [13, 7, 13, 7, 5, 3, 11, 2],
               [11, 2, 11, 13, 17],
               [7, 2, 13, 3, 11, 3, 3, 11],
               [11, 13, 17, 7, 19, 19],
               [7, 2, 7, 5],
               [17, 11, 17, 7, 13],
               [11, 7, 11, 11, 11],
               [7, 5, 17, 2, 2, 11, 11, 7]
         ];

      SequenceAssert.AreEqual(expected, permutations, assertAreEqualAction: (exp, act, msg)
            => SequenceAssert.AreEqual(exp, act, assertAreEqualAction: null, msg));
   }
}
