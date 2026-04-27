using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface IApplicative<F> : IFunctor<F> where F : IApplicative<F> {
   // pure :: a -> f a
   public static abstract K<F, A> Pure<A>(A a);

   // (<*>) :: f (a -> b) -> f a -> f b
   public static abstract K<F, B> Apply<A, B>(K<F, A> fa, K<F, Func<A, B>> ffunc);
}

