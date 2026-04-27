using System;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers;

public record KList<A>(ImmutableList<A> List) : K<KList, A>;


public partial class KList : IFunctor<KList> {
   public static K<KList, B> FMap<A, B>(K<KList, A> a, Func<A, B> func)
      => new KList<B>(((KList<A>)a).List.Select(func)
                                   .ToImmutableList());
}


partial class KList : IApplicative<KList> {
   public static K<KList, A> Pure<A>(A a)
      => new KList<A>([a]);


   public static K<KList, B> Apply<A, B>(K<KList, Func<A, B>> ffunc, K<KList, A> fa)
      => new KList<B>((from a in fa.As().List
                       from func in ffunc.As().List
                       select func(a)
                      ).ToImmutableList());
}


public static class KListExtensions {
   public static KList<A> As<A>(this K<KList, A> ma) => (KList<A>)ma;


   public static K<KList, B> FMap<A, B>(this K<KList, A> a, Func<A, B> func)
         where A : IEquatable<A>
         where B : IEquatable<B>
      => KList.FMap(a, func);
}
