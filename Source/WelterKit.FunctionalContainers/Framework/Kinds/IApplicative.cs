using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface IApplicative<F> : IFunctor<F> where F : IApplicative<F> {
   // pure :: a -> f a
   public static abstract K<F, A> Pure<A>(A a);

   // (<*>) :: f (a -> b) -> f a -> f b
   public static abstract K<F, B> Apply<A, B>(K<F, A> fa, K<F, Func<A, B>> ffunc);
}



public static class ApplicativeExtensions {
   public static K<F, B> Apply<F, A, B>(this K<F, A> fa, K<F, Func<A, B>> ffunc) where F : IApplicative<F> => F.Apply(fa, ffunc);
   public static K<F, B> Apply<F, A, B>(this K<F, Func<A, B>> ffunc, K<F, A> fa) where F : IApplicative<F> => F.Apply(fa, ffunc);

   // Func<K<F, A>, Func<K<F, Func<A, B>>, K<F, B>>> applyCurried1 = (K<F, A> fa) => (K<F, Func<A, B>> ffunc) => F.Apply(fa, ffunc);
   // Func<K<F, Func<A, B>>, Func<K<F, A>, K<F, B>>> applyCurried2 = (K<F, Func<A, B>> ffunc) => (K<F, A> fa) => F.Apply(fa, ffunc);
}
