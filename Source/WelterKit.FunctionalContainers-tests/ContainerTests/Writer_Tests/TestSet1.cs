using System;
using System.Numerics;
using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;
using WelterKit.FunctionalContainers;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.Writer_tests;

internal static class TestSet1 {
   public static Writer<StringAccumulator, decimal> Writer_sum3 { get; }
      = getFinal(sum3);

   public static Writer<StringAccumulator, Func<decimal, int>> Func_decimal_int { get; }
      = new(() => (new StringAccumulator(["123"]),
                   x => (int)decimal.Ceiling(x)));

   public static Writer<StringAccumulator, Func<int, string>> Func_int_string { get; }
      = new(() => (new StringAccumulator(["987"]),
                   x => x.ToString()));



   static Writer<StringAccumulator, decimal> getFinal(Func<decimal, decimal, decimal, decimal> combineResults)
      => Writer<StringAccumulator>.Pure(Fn.Curry(combineResults))
                                  .Apply(startWith(42.42M))
                                  .Apply(then(42.42M))
                                  .Apply(andFinally(42.42M))
                                  .As();

   static Writer<StringAccumulator, decimal> startWith(decimal value)
      => new(() => (new StringAccumulator([$"Starting with: {value}"]), value));

   static Writer<StringAccumulator, decimal> then(decimal value) {
      decimal h = value / 2;
      return new(() => (new StringAccumulator([$"Then, halved: {h}"]), h));
   }

   static Writer<StringAccumulator, decimal> andFinally(decimal value) {
      decimal t = value * 3;
      return new(() => (new StringAccumulator([$"And, finally, tripled: {t}"]), t));
   }

   static N sum3<N>(N a, N b, N c) where N : INumber<N> => a + b + c;
}
