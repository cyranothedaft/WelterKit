using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public abstract class Laws_Functor<L> : Laws_Functor_Tests<Either<L>> {
   protected override void AssertAreEqual<A>(K<Either<L>, A> expected, K<Either<L>, A> actual)
      => Assert.AreEqual(expected.As(),
                         actual  .As());


   protected override K<Either<L>, int    >[] GetTestSubjects (int    [] rightValues) => getVariations(rightValues);
   protected override K<Either<L>, float  >[] GetTestSubjects (float  [] rightValues) => getVariations(rightValues);
   protected override K<Either<L>, string >[] GetTestSubjects (string [] rightValues) => getVariations(rightValues);
   protected override K<Either<L>, string?>[] GetTestSubjectsn(string?[] rightValues) => getVariations(rightValues);


   private K<Either<L>, R>[] getVariations<R>(R[] rightValues)
      => rightValues.Select(toRight)
                    .Concat(LeftValues.Select(toLeft<R>))
                    .ToArray<K<Either<L>, R>>();


   protected abstract IEnumerable<L> LeftValues { get; }

   private static Either<L, R> toLeft <R>(L value) => new Left <L, R>(value);
   private static Either<L, R> toRight<R>(R value) => new Right<L, R>(value);
}



[TestClass]
public class Laws_Functor_string : Laws_Functor<string> {
   protected override IEnumerable<string> LeftValues { get; } = TestValues._string;
}


[TestClass]
public class Laws_Functor_record : Laws_Functor<Laws_Functor_record.CustomRecord> {
   public record CustomRecord(int IntValue, string StringValue);

   protected override IEnumerable<CustomRecord> LeftValues { get; }
      = from i in TestValues._int
        from s in TestValues._string
        select new CustomRecord(i, s);
}
