using System;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;

// TODO: abstract or interface
public interface State<S, A> : K<State, A> {

}


public partial class State : IFunctor<State> {
   public static K<State, B> FMap<A, B>(K<State, A> a, Func<A, B> func)
      => new State<B>(((State<A>)a).List.Select(func)
                                   .ToImmutableList());
}


partial class State : IApplicative<State> {
   public static K<State, A> Pure<A>(A a)
      => new State<A>([a]);


   public static K<State, B> Apply<A, B>(K<State, Func<A, B>> ffunc, K<State, A> fa)
      => new State<B>((from a in fa.As().List
                       from func in ffunc.As().List
                       select func(a)
                      ).ToImmutableList());
}



partial class State : IMonad<State> {
   // bind :: (M a) -> (a -> M b) -> (M b)
   // ma >>= f  =  join (fmap f ma)
   public static K<State, B> Bind<A, B>(K<State, A> ma, Func<A, K<State, B>> f)
      => join(FMap(ma, f));


   // join :: m (m a) -> m a
   private static K<State, A> join<A>(K<State, K<State, A>> mma)
      => new State<A>(mma.As().List.SelectMany(innerList => innerList.As().List)
                                   .ToImmutableList());

}


public static class StateExtensions {
   public static State<A> As<A>(this K<State, A> ma) => (State<A>)ma;
}
