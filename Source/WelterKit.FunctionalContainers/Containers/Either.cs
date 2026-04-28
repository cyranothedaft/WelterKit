using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;


public abstract partial record Either<L, R> : K<Either<L>, R>;
public          partial record Left  <L, R>(L Value) : Either<L, R>;
public          partial record Right <L, R>(R Value) : Either<L, R>;


public partial class Either<L> : IFunctor<Either<L>> {
   public static K<Either<L>, RNew> FMap<R, RNew>(K<Either<L>, R> fa, Func<R, RNew> func)
      => fa.As() switch
         {
            Left <L,R> lt => new Left <L, RNew>(lt.Value),
            Right<L,R> rt => new Right<L, RNew>(func(rt.Value)),
            _             => throw new ArgumentOutOfRangeException(nameof( fa ))
         };
}


public partial class Either<L> : IApplicative<Either<L>> {
   public static K<Either<L>, A> Pure<A>(A a)
      => new Right<L, A>(a);


   public static K<Either<L>, B> Apply<A, B>(K<Either<L>, Func<A, B>> ffunc, K<Either<L>, A> fa)
      => fa.As() switch
         {
            Left <L, A> lt => new Left<L, B>(lt.Value),
            Right<L, A> rt => (ffunc.As() switch {
                                       Left <L, Func<A, B>> ltf => new Left <L, B>(ltf.Value),
                                       Right<L, Func<A, B>> rtf => new Right<L, B>(rtf.Value(rt.Value)),
                                       _                        => throw new ArgumentOutOfRangeException()
                                    }),
            _              => throw new ArgumentOutOfRangeException()
         };
}


public partial class Either<L> : IMonad<Either<L>> {
   public static K<Either<L>, B> Bind<A, B>(K<Either<L>, A> ma, Func<A, K<Either<L>, B>> f)
      => ma.As() switch
         {
            Left <L, A> lt => new Left<L, B>(lt.Value),
            Right<L, A> rt => f(rt.Value),
            _              => throw new ArgumentOutOfRangeException()
         };
}



public static class Either {
   public static Either<L, RNew> FMap<L, R, RNew>(Either<L, R> fa, Func<R, RNew> func) => Either<L>.FMap(fa, func).As();
}



public partial record Either<L, R> {
   public static implicit operator Either<L, R>(L leftValue ) => new Left <L, R>(leftValue);
   public static implicit operator Either<L, R>(R rightValue) => new Right<L, R>(rightValue);
}


public static class EitherExtensions {
   public static Either<L,R> As<L, R>(this K<Either<L>, R> ma) => (Either<L, R>)ma;
}
