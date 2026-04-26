using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers;

public abstract partial record Maybe<A> : K<Maybe, A>;
public          partial record Some <A>(A Value) : Maybe<A> where A : IEquatable<A>;
public          partial record None <A>          : Maybe<A>;


public class Maybe : IFunctor<Maybe> {
   public static K<Maybe, B> FMap<A, B>(K<Maybe, A> a, Func<A, B> func)
      where A : IEquatable<A>
      where B : IEquatable<B>
      => (Maybe<A>)a switch
         {
            None<A>      => new None<B>(),
            Some<A> some => new Some<B>(func(some.Value)),
            _            => throw new ArgumentOutOfRangeException(nameof( a ))
         };
}
