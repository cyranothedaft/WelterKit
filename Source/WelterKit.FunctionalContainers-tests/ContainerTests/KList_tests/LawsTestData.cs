using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Support;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.Std.StaticUtilities;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

public class LawsTestData : ILawsTestData<KList> {
    public void AssertAreEqual<A>(K<KList, A> expected, K<KList, A> actual)
       => CollectionAssert.AreEqual(expected.As().List,
                                    actual  .As().List);


    public K<KList, int    >[] GetTestSubjects (int    [] values) => getVariations(values);
    public K<KList, float  >[] GetTestSubjects (float  [] values) => getVariations(values);
    public K<KList, string >[] GetTestSubjects (string [] values) => getVariations(values);
    public K<KList, string?>[] GetTestSubjectsn(string?[] values) => getVariations(values);


    private K<KList, A>[] getVariations<A>(A[] values)
       => ((A[][])
                [
                   // always test these
                   Array.Empty<A>(),
                   values,
                   values.RepeatSequence(2).ToArray()
                ])
         .Concat(ListMaker.ChoosePermutations(values,
                                              new RandomIntSequenceGenerator(42 * 42),
                                              count: 12,
                                              minLength: 1,
                                              maxLength: 20))
         .Select(toKList)
         .ToArray<K<KList, A>>();


    private static KList<A> toKList<A>(IEnumerable<A> source)
       => new(source.ToImmutableList());
}
