using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;


public abstract partial record Maybe<A> : K<Maybe, A>;
public          partial record Some <A>(A Value) : Maybe<A>;
public          partial record None <A>          : Maybe<A>;


public partial class Maybe : IFunctor<Maybe> {
   public static K<Maybe, B> FMap<A, B>(K<Maybe, A> fa, Func<A, B> func)
      => fa.As() switch
         {
            None<A>      => new None<B>(),
            Some<A> some => new Some<B>(func(some.Value)),
            _            => throw new ArgumentOutOfRangeException(nameof( fa ))
         };
}



public partial class Maybe : IApplicative<Maybe> {
   public static K<Maybe, A> Pure<A>(A a)
      => new Some<A>(a);


   public static K<Maybe, B> Apply<A, B>(K<Maybe, Func<A, B>> ffunc, K<Maybe, A> fa)
      => fa.As() switch
         {
            None<A> => new None<B>(),
            Some<A> some1 => (ffunc.As() switch
                                   {
                                      None<Func<A, B>>       => new None<B>(),
                                      Some<Func<A, B>> some2 => new Some<B>(some2.Value(some1.Value)),
                                      _                      => throw new ArgumentOutOfRangeException()
                                   }),
            _ => throw new ArgumentOutOfRangeException()
         };
}



public static class MaybeExtensions {
   public static Maybe<A> As<A>(this K<Maybe, A> ma) => (Maybe<A>)ma;


   // public static K<Maybe, B> FMap<A, B>(this K<Maybe, A> a, Func<A, B> func)
   //    => Maybe.FMap(a, func);

}
