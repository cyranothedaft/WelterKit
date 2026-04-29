using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface IMonad<M> : IFunctor<M>, IApplicative<M> where M : IMonad<M> {
   // return :: a -> M a 
   public static virtual K<M, A> Return<A>(A a)
      => M.Pure(a);

   // bind :: (M a) -> (a -> M b) -> (M b)
   public static abstract K<M, B> Bind<A, B>(K<M, A> ma, Func<A, K<M, B>> f);
}



public static class MonadExtensions {
   public static K<M, A> Return<M, A>(this A a)                             where M : IMonad<M> => M.Return(a);
   public static K<M, B> Bind<M, A, B>(this K<M, A> ma, Func<A, K<M, B>> f) where M : IMonad<M> => M.Bind(ma, f);
}
