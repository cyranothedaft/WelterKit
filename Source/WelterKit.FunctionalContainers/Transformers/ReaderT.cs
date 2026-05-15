using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;



namespace WelterKit.FunctionalContainers.Transformers;

// newtype ReaderT r m a = ReaderT { runReaderT :: r -> m a }
public record ReaderT<R, M, A>(Func<R, K<M, A>> RunReaderT) : K<ReaderT<R, M>, A> where M : IMonad<M> {
   public static K<M, A> runReaderT(ReaderT<R, M, A> ma, R r) => ma.RunReaderT(r);
}



public partial class ReaderT<R,M> where M : IMonad<M>;


partial class ReaderT<R, M> : IFunctor<ReaderT<R, M>> where M : IMonad<M> {
   public static K<ReaderT<R, M>, B> FMap<A, B>(K<ReaderT<R, M>, A> fa, Func<A, B> func) {
      throw new NotImplementedException();
   }
}


partial class ReaderT<R, M> : IApplicative<ReaderT<R, M>> where M : IMonad<M> {
   // pure a = ReaderT (\_ -> pure a)
   public static K<ReaderT<R, M>, A> Pure<A>(A a)
      => new ReaderT<R, M, A>(_ => M.Pure(a));

   // (ReaderT f) <*> (ReaderT v) = ReaderT (\r -> f r <*> v r)
   public static K<ReaderT<R, M>, B> Apply<A, B>(K<ReaderT<R, M>, Func<A, B>> ffunc, K<ReaderT<R, M>, A> fa) 
   =>
}


partial class ReaderT<R, M> : IMonad<ReaderT<R, M>> where M : IMonad<M> {
   public static K<ReaderT<R, M>, B> Bind<A, B>(K<ReaderT<R, M>, A> ma, Func<A, K<ReaderT<R, M>, B>> f) {
      throw new NotImplementedException();
   }
}



public static class ReaderTExtensions {
   public static ReaderT<R, M, A> As<R, M, A>(this K<ReaderT<R, M>, A> ma) where M : IMonad<M> => (ReaderT<R, M, A>)ma;
}
