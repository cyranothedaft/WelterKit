using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface IFunctor<F> where F : IFunctor<F> {
   public static abstract K<F, B> FMap<A, B>(K<F, A> a, Func<A, B> func);
}
