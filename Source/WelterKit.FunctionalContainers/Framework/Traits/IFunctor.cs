using System;

namespace WelterKit.FunctionalContainers.Framework.Traits;

public interface IFunctor<F> where F : IFunctor<F> {
   public static abstract K<F, B> FMap<A, B>(K<F, A> fa, Func<A, B> func);
}


public static class FunctorExtensions {
   public static K<F, B> FMap<F, A, B>(this K<F, A> fa, Func<A, B> func) where F : IFunctor<F> => F.FMap(fa, func);
}
