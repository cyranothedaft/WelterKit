using System;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.FunctionalContainers_tests.Support;

public interface ISequenceGenerator {
   int Next();


   IEnumerable<int> NextN(int count)
      => EnumerateEndless().Take(count);


   // IEnumerable<int> NextN(int count, int modulo)
   //    => NextN(count)
   //         .Select(n => n % modulo);


   IEnumerable<A> GetItems<A>(IList<A> chooseFrom, int countToChoose)
      => NextN(countToChoose)
           .Select(i => chooseFrom[i % chooseFrom.Count]);


   IEnumerable<int> EnumerateEndless() {
      while (true) {
         yield return Next();
      }
      // ReSharper disable once IteratorNeverReturns
   }
}



public class RecursiveSequenceGenerator : ISequenceGenerator {
   private int _nextValue;
   private readonly Func<int, int> _generator;


   public RecursiveSequenceGenerator(int initialValue, Func<int, int> generator) {
      _nextValue = initialValue;
      _generator = generator;
   }


   public int Next() {
      int value = _nextValue;
      _nextValue = _generator(value);
      return value;
   }
}



public class RandomIntSequenceGenerator : ISequenceGenerator {
   private readonly Random _rng;

   public RandomIntSequenceGenerator(int? seed = null) {
      _rng = seed.HasValue
                   ? new Random(seed.Value)
                   : new Random();

   }

   public int Next() => _rng.Next();
}
