using System;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;


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



partial class KList : IMonad<KList> {
   // bind :: (M a) -> (a -> M b) -> (M b)
   // ma >>= f  =  join (fmap f ma)
   public static K<KList, B> Bind<A, B>(K<KList, A> ma, Func<A, K<KList, B>> f)
      => join(FMap(ma, f));


   // join :: m (m a) -> m a
   private static K<KList, A> join<A>(K<KList, K<KList, A>> mma)
      => new KList<A>(mma.As().List.SelectMany(innerList => innerList.As().List)
                                   .ToImmutableList());

}


public static class KListExtensions {
   public static KList<A> As<A>(this K<KList, A> ma) => (KList<A>)ma;
}
