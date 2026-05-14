using System;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.FunctionalContainers_tests.Support;


internal static class ListMaker {
   public static IEnumerable<A[]> ChoosePermutations<A>(A[] values, ISequenceGenerator sequence, int count, int minLength,int maxLength) {
      return getListSizes()
            .ToList() // ensures this entire sequence gets materialized first, for consistency
             // TODO: construct multiple lists of each size
            .Select(size => MakeListOfSize(sequence, values, size));

      IEnumerable<int> getListSizes()
         => sequence.NextN(count)
                    .Select(n => (n % (maxLength - minLength + 1)) + minLength);
   }


   public static A[] MakeListOfSize<A>(ISequenceGenerator sequence, A[] possibleElements, int listSize)
      => sequence.GetItems(possibleElements, listSize)
                 .ToArray();
}
