// using System;
//
// namespace WelterKit.FunctionalContainers.Framework.Kinds;
//
// public interface IMonoid<M> : ISemigroup<M>  where M : IMonoid<M> {
//    // return :: a -> M a 
//    // aka "unit"
//    public static virtual K<M, A> Return<A>(A a)
//       => M.Pure(a);
//
//    // bind :: (M a) -> (a -> M b) -> (M b)
//    public static abstract K<M, B> Bind<A, B>(K<M, A> ma, Func<A, K<M, B>> f);
//
//    // // equivalent of Haskell's ">>" operator
//    // public static virtual K<M, B> Bind2<A, B>(K<M, A> ma, Func<K<M, B>> f) => M.Bind(ma, _ => f());
// }
//
//
//
// public static class MonoidExtensions {
//    public static K<M, A> Return<M, A>   (this A a)                            where M : IMonoid<M> => M.Return(a);
//    public static K<M, B> Bind  <M, A, B>(this K<M, A> ma, Func<A, K<M, B>> f) where M : IMonoid<M> => M.Bind(ma, f);
//    // public static K<M, B> Bind2 <M, A, B>(this K<M, A> ma, Func<K<M, B>> f)    where M : IMonoid<M> => M.Bind2(ma, f);
// }
