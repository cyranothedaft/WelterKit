using System;

namespace WelterKit.FunctionalContainers.Framework.Traits;

public interface ITraversible<T> : IFunctor<T>, IFoldable<T>
                                 where T : ITraversible<T> {
   // traverse :: (Applicative f, Traversable t) => (a -> f b) -> t a -> f (t b)
   public static abstract K<F, K<T, B>> Traverse<F, A, B>(Func<A, K<F, B>> f, K<T, A> a) where F : IApplicative<F>;
}



public static class TraversibleExtensions {
   public static K<F, K<T, B>> Traverse<T, F, A, B>(this K<T, A> a, Func<A, K<F, B>> f) where T : ITraversible<T>
                                                                                        where F : IApplicative<F>
      => T.Traverse(f, a);
}
