using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.FunctionalContainers_tests.Support;


internal static class ListMaker {
   public static IEnumerable<A[]> ChoosePermutations<A>(A[] values, ISequenceGenerator sequence, int count, int minLength,int maxLength) {
      // int[] possibleSizes = Enumerable.Range(minLength, maxLength - minLength + 1)
      //                                        .ToArray();
      int lengthSpan = maxLength - minLength;
      IEnumerable<int> sizesToTest = sequence.NextN(count)
                                             .Select(n => (n % lengthSpan) + minLength);

Debug.WriteLine(sizesToTest.JoinString(", ", n => n.ToString()));

      // construct multiple lists of each size
      return sizesToTest.Select(size => MakeListOfSize(sequence, values, size));
   }


   public static A[] MakeListOfSize<A>(ISequenceGenerator sequence, A[] possibleElements, int listSize)
      => sequence.GetItems(possibleElements, listSize)
                 .ToArray();
   //=> Enumerable.Range(0, listSize)
   //             .Select(_ => possibleElements[sequence.Next()])
   //             .ToArray();
}
