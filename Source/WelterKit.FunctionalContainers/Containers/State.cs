using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;

// newtype State s a = State { runState :: s -> (s, a) }

// TODO: abstract or interface (or maybe just extract an interface for the method signatures)
// TODO: figure out a 'newstate'-like way of avoiding actually creating new instances of this
public record State<S, A>(Func<S, (S, A)> runState) : K<State<S>, A>;


public partial class State<S> {
   // get :: State s s
   // get = State (\s -> (s, s))
   public static State<S, S> get { get; }
      = new(s => (s, s));


   // put :: s -> State s ()
   // put s = State $ \_ -> ((), s). 
   public static State<S, Unit> put(S s)
      => new(_ => (s, Unit.Value));


   // modify :: (s -> s) -> State s ()
   // modify f = State (\s -> ((), f s))
   public static State<S, Unit> modify(Func<S, S> f)
      => new(s => (f(s), Unit.Value));

   // TODO: also modify' ?
}


partial class State<S> : IFunctor<State<S>> {
   public static K<State<S>, B> FMap<A, B>(K<State<S>, A> fa, Func<A, B> func)
      => new State<S, B>(s => {
                            (S newState, A result) = fa.As().runState(s);
                            return (newState, func(result));
                         });
}


partial class State<S> : IApplicative<State<S>> {
   public static K<State<S>, A> Pure<A>(A a)
      => new State<S, A>(s => (s, a));


   public static K<State<S>, B> Apply<A, B>(K<State<S>, Func<A, B>> ffunc, K<State<S>, A> fa)
      => new State<S, B>(s => {
                            (S s1, Func<A, B> f) = ffunc.As().runState(s);
                            (S s2, A x)          = fa.As().runState(s1);
                            return (s2, f(x));
                         });
}



partial class State<S> : IMonad<State<S>> {
   // m >>= f = State $ \s ->
   // let (val, nextState) = runState m s  -- 1. Run the first computation with the current state
   //     m'               = f val         -- 2. Apply the function to the resulting value
   // in runState m' nextState             -- 3. Run the second computation with the new state


   public static K<State<S>, B> Bind<A, B>(K<State<S>, A> ma, Func<A, K<State<S>, B>> f)
      => new State<S, B>(s => {
                            (S nextState, A val) = ma.As().runState(s);
                            State<S, B> m2 = f(val).As();
                            return m2.runState(nextState);
                         });
}


public static class StateExtensions {
   public static State<S, A> As<S, A>(this K<State<S>, A> ma) => (State<S, A>)ma;
}
