using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers.Containers;


public abstract partial record Maybe<A> : K<Maybe, A>;
public          partial record Some <A>(A Value) : Maybe<A>;
public          partial record None <A>          : Maybe<A>;



partial record Maybe<A> {
   public abstract B Match<B>(Func<A, B> funcIfSome, Func<B> funcIfNone);
}

partial record Some<A> {
   public override B Match<B>(Func<A, B> funcIfSome, Func<B> funcIfNone) => funcIfSome(this.Value);
}

partial record None<A> {
   public override B Match<B>(Func<A, B> funcIfSome, Func<B> funcIfNone) => funcIfNone();
}


public partial class Maybe : IFunctor<Maybe> {
   public static K<Maybe, B> FMap<A, B>(K<Maybe, A> fa, Func<A, B> func)
      => fa.As() switch
         {
            None<A>      => new None<B>(),
            Some<A> some => new Some<B>(func(some.Value)),
            _            => throw new ArgumentOutOfRangeException(nameof( fa ))
         };
}



partial class Maybe : IApplicative<Maybe> {
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



partial class Maybe : IMonad<Maybe> {
   public static K<Maybe, B> Bind<A, B>(K<Maybe, A> ma, Func<A, K<Maybe, B>> f)
      => ma.As() switch
         {
            None<A>      => new None<B>(),
            Some<A> some => f(some.Value),
            _            => throw new ArgumentOutOfRangeException()
         };
}


public static class MaybeExtensions {
   public static Maybe<A> As<A>(this K<Maybe, A> ma) => (Maybe<A>)ma;

   public static Some<A> AsSome<A>(this A value) => new Some<A>(value);
}
