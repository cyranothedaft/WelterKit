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
   // fmap f (ReaderT g) = ReaderT (\r -> fmap f (g r))
   public static K<ReaderT<R, M>, B> FMap<A, B>(K<ReaderT<R, M>, A> mg, Func<A, B> f)
      => new ReaderT<R, M, B>(r => M.FMap(mg.As().RunReaderT(r),
                                          f));
}


partial class ReaderT<R, M> : IApplicative<ReaderT<R, M>> where M : IMonad<M> {
   // pure a = ReaderT (\_ -> pure a)
   public static K<ReaderT<R, M>, A> Pure<A>(A a)
      => new ReaderT<R, M, A>(_ => M.Pure(a));

   // (ReaderT f) <*> (ReaderT v) = ReaderT (\r -> f r <*> v r)
   public static K<ReaderT<R, M>, B> Apply<A, B>(K<ReaderT<R, M>, Func<A, B>> f, K<ReaderT<R, M>, A> v)
      => new ReaderT<R, M, B>(r => M.Apply(f.As().RunReaderT(r),
                                           v.As().RunReaderT(r)));
}


partial class ReaderT<R, M> : IMonad<ReaderT<R, M>> where M : IMonad<M> {
   // ReaderT ma >>= f = ReaderT $ \r -> ma r >>= \a -> runReaderT (f a) r
   public static K<ReaderT<R, M>, B> Bind<A, B>(K<ReaderT<R, M>, A> ma, Func<A, K<ReaderT<R, M>, B>> f)
      => new ReaderT<R, M, B>(r => M.Bind(ma.As().RunReaderT(r),
                                          a => ReaderT<R, M, B>.runReaderT(f(a).As(),
                                                                           r)));
}



public static class ReaderTExtensions {
   public static ReaderT<R, M, A> As<R, M, A>(this K<ReaderT<R, M>, A> ma) where M : IMonad<M> => (ReaderT<R, M, A>)ma;
}


/*
for Reader:
import Control.Monad.Reader

instance Monad (Reader r) where
    -- return :: a -> Reader r a
    return x = Reader (\_ -> x)

    -- (>>=) :: Reader r a -> (a -> Reader r b) -> Reader r b
    (Reader f) >>= k = Reader $ \r -> runReader (k (f r)) r
 */