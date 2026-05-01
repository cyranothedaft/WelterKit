// using System;
// using WelterKit.FunctionalContainers.Framework;
// using WelterKit.FunctionalContainers.Framework.Kinds;
//
//
// namespace WelterKit.FunctionalContainers.Containers;
//
// // newtype Writer w a = Writer { runWriter :: (a, w) } 
//
// // instance (Monoid w, Monad m) => Monad (WriterT w m) where
// //    return a = writer (a, mempty)
// //    m >>= k  = WriterT $ do
// //        (a, w)  <- runWriterT m
// //        (b, w') <- runWriterT (k a)
// //        return (b, w `mappend` w')
//
// // TODO: figure out a 'newtype'-like way of avoiding actually creating new instances of this
// public record Writer<W, A>((W writer, A value) runWriter) : K<Writer<W>, A>;
//
//
// public partial class Writer<W> {
// }
//
//
// partial class Writer<W> : IFunctor<Writer<W>> {
//    // fmap f (Writer (a, w)) = Writer (f a, w)
//    public static K<Writer<W>, B> FMap<A, B>(K<Writer<W>, A> fa, Func<A, B> func) {
//       (W w, A a) = fa.As().runWriter;
//       return new Writer<W, B>((w, func(a)));
//    }
// }
//
//
// partial class Writer<W> : IApplicative<Writer<W>>, IMonoid<Writer<W>> {
//    // pure x = (mempty, x)
//    public static K<Writer<W>, A> Pure<A>(A a)
//       => new Writer<W, A>(())
//
//
//    public static K<Writer<W>, B> Apply<A, B>(K<Writer<W>, Func<A, B>> ffunc, K<Writer<W>, A> fa)
//       => new Writer<W, B>(s => {
//                             (W s1, Func<A, B> f) = ffunc.As().runWriter(s);
//                             (W s2, A x)          = fa.As().runWriter(s1);
//                             return (s2, f(x));
//                          });
// }
//
//
//
// partial class Writer<W> : IMonad<Writer<W>> {
//    // m >>= f = Writer $ \s ->
//    // let (val, nextWriter) = runWriter m s  -- 1. Run the first computation with the current state
//    //     m'               = f val         -- 2. Apply the function to the resulting value
//    // in runWriter m' nextWriter             -- 3. Run the second computation with the new state
//
//
//    public static K<Writer<W>, B> Bind<A, B>(K<Writer<W>, A> ma, Func<A, K<Writer<W>, B>> f)
//       => new Writer<W, B>(s => {
//                             (W nextWriter, A val) = ma.As().runWriter(s);
//                             Writer<W, B> m2 = f(val).As();
//                             return m2.runWriter(nextWriter);
//                          });
// }
//
//
// public static class WriterExtensions {
//    public static Writer<S, A> As<S, A>(this K<Writer<S>, A> ma) => (Writer<S, A>)ma;
// }
