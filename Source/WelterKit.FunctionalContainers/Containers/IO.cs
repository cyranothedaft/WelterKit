using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers.Containers;


// newtype IO a = IO (State# RealWorld -> (# State# RealWorld, a #))
public partial record IO<A> : K<IO, A>() {



    public static implicit operator IO<A>(Pure<A> ma) =>
        IO.pure(ma.Value);

    public static implicit operator IO<A>(Error error) =>
        IO.lift(_ => error.Throw<A>());

    public static implicit operator IO<A>(Fail<Error> ma) =>
        IO.lift(() => ma.Value.Throw<A>());

    public static implicit operator IO<A>(Fail<Exception> ma) =>
        IO.lift(() => ma.Value.Rethrow<A>());

    public static implicit operator IO<A>(Lift<EnvIO, A> ma) =>
        IO.lift(ma.Function);

    public static implicit operator IO<A>(Lift<A> ma) =>
        IO.lift(ma.Function);

    public A Run()
    {
        // RunAsync can run completely synchronously and without the creation of an async/await state-machine, so calling
        // it for operations that are completely synchronous has no additional overhead for us.  Therefore, calling it
        // directly here and then unpacking the `ValueTask` makes sense to reduce code duplication.

        var task = RunAsync();
        if (task.IsCompleted) return task.Result;

        // If RunAsync really had to do some asynchronous work, then make sure we use the awaiter and get its result
        return task.GetAwaiter().GetResult();
    }
}



public partial class IO : IFunctor<IO> {
   public static K<IO, B> FMap<A, B>(K<IO, A> ma, Func<A, B> f)
      => ma.As().FMap(f);
}



partial class IO : IApplicative<IO> {
   public static K<IO, A> Pure<A>(A a)
      => new IO<A>(a);


   public static K<IO, B> Apply<A, B>(K<IO, Func<A, B>> ffunc, K<IO, A> fa)
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



partial class IO : IMonad<IO> {
   // return :: a -> IO a

   // bindIO :: IO a -> (a -> IO b) -> IO b
   // bindIO action1 function2 = \world0 ->
   //     case action1 world0 of
   //         (# world1, result1 #) -> function2 result1 world1

   // (IO m) >>= f = IO $ \s -> 
   //     case m s of 
   //         (# s', a #) -> case f a of 
   //             IO m' -> m' s'
   public static K<IO, B> Bind<A, B>(K<IO, A> ma, Func<A, K<IO, B>> f)
      => new IO<B>(s=>) ;
}


public static class IOExtensions {
   public static IO<A> As<A>(this K<IO, A> ma) => (IO<A>)ma;

   public static Some<A> AsSome<A>(this A value) => new Some<A>(value);
}










public record Lift<A>(Func<A> Function)
{
    public K<F, A> ToApplicative<F>() 
        where F : IApplicative<F> =>
        F.Pure(Unit.Value).FMap(_ => Function());

    public IO<A> ToIO() =>
        IO.lift(_ => Function());

    public Eff<A> ToEff() =>
        Eff<A>.Lift(Function);

    public Eff<RT, A> ToEff<RT>() =>
        Eff<RT, A>.Lift(Function);

    public Lift<B> Map<B>(Func<A, B> f) =>
        new (() => f(Function()));

    public Lift<B> Bind<B>(Func<A, Lift<B>> f) =>
        new(() => f(Function()).Function());

    public Lift<B> Bind<B>(Func<A, Pure<B>> f) =>
        new (() => f(Function()).Value);

    public K<M, B> Bind<M, B>(Func<A, K<M, B>> f)
        where M : IMonad<M> =>
        M.Pure(Unit.Value).FMap(_ => Function()).Bind(f);

    public IO<B> Bind<B>(Func<A, IO<B>> f) =>
        ToIO().Bind(f);

    public Lift<B> Select<B>(Func<A, B> f) =>
        Map(f);

    public IO<C> SelectMany<B, C>(Func<A, IO<B>> bind, Func<A, B, C> project) => 
        ToIO().SelectMany(bind, project);

    public K<M, C> SelectMany<M, B, C>(Func<A, K<M, B>> bind, Func<A, B, C> project)  
        where M : IMonad<M> =>
        Bind(x => bind(x).FMap(y => project(x, y)));

    public Lift<C> SelectMany<B, C>(Func<A, Lift<B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));

    public Lift<C> SelectMany<B, C>(Func<A, Pure<B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
}

public record Lift<A, B>(Func<A, B> Function)
{
    public Lift<A, C> Map<C>(Func<B, C> f) =>
        new (x => f(Function(x)));

    public Lift<A, C> Bind<C>(Func<B, Lift<A, C>> f) =>
        new(x => f(Function(x)).Function(x));

    public Lift<A, C> Bind<C>(Func<B, Pure<C>> f) =>
        new (x => f(Function(x)).Value);

    public Lift<A, C> Select<C>(Func<B, C> f) =>
        Map(f);

    public Lift<A, D> SelectMany<C, D>(Func<B, Pure<C>> bind, Func<B, C, D> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
}
