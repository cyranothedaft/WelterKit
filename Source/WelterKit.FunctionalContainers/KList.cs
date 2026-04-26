using System;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers;

public record KList<A>(ImmutableList<A> List) : K<KList, A>;


public class KList : IFunctor<KList> {
   public static K<KList, B> FMap<A, B>(K<KList, A> a, Func<A, B> func)
      where A : IEquatable<A>
      where B : IEquatable<B>
      => new KList<B>(((KList<A>)a).List.Select(func)
                                   .ToImmutableList());
}
