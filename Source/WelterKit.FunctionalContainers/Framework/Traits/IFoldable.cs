using System;

namespace WelterKit.FunctionalContainers.Framework.Traits;

public interface IFoldable<M> where M : IFoldable<M> {
   // foldr :: (a -> b -> b) -> b -> t a -> b
   public static abstract B Foldr<A, B>(Func<A, B, B> fold, B initialValue, K<M, A> ma);
}


public static class FoldableExtensions {
   public static B Foldr<M, A, B>(this K<M, A> ma, Func<A, B, B> fold, B initialValue) where M : IFoldable<M> => M.Foldr(fold, initialValue, ma);
}
