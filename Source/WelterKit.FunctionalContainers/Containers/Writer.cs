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
//    // get :: Writer s s
//    // get = Writer (\s -> (s, s))
//    // a state transformer that wraps a state transformer function which takes
//    //   the current state, doesn't alter it, and returns it as a value.
//    public static Writer<W, W> get { get; }
//       = new(s => (s, s));
//
//
//    // put :: s -> Writer s ()
//    // put s = Writer $ \_ -> ((), s). 
//    // a function which returns a state transformer given a state value
//    public static Writer<W, Unit> put(W s)
//       => new(_ => (s, Unit.Value));
//
//
//    // modify :: (s -> s) -> Writer s ()
//    // modify f = Writer (\s -> ((), f s))
//    public static Writer<W, Unit> modify(Func<W, W> f)
//       => new(s => (f(s), Unit.Value));
//
//    // TODO: also modify' ?
// }
//
//
// partial class Writer<W> : IFunctor<Writer<W>> {
//    public static K<Writer<W>, B> FMap<A, B>(K<Writer<W>, A> fa, Func<A, B> func) {
//       (W newWriter, A result) = fa.As().runWriter;
//       return new Writer<W, B>((newWriter, func(result)));
//    }
// }
//
//
// partial class Writer<W> : IApplicative<Writer<W>> {
//    public static K<Writer<W>, A> Pure<A>(A a)
//       => new Writer<W, A>(s => (s, a));
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
