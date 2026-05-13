using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers.Containers;



namespace WelterKit.FunctionalContainers_tests.Support;

public interface ISequenceGenerator {
   int Next();


   IEnumerable<int> NextN(int count)
      => EnumerateEndless().Take(count);


   IEnumerable<A> GetItems<A>(IList<A> chooseFrom, int countToChoose)
      => NextN(countToChoose).Select(i => chooseFrom[i]);


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


   private RandomIntSequenceGenerator(Maybe<int> seed_) {
      _rng = seed_.Match(seed => new Random(seed),
                         ()   => new Random());
   }


   public int Next() => _rng.Next();

   public int Next(int minValue, int maxValue) => (Next() % (maxValue - minValue)) + minValue;
}
