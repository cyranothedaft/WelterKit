using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers.Containers;

// newtype Reader r a = Reader { runReader :: r -> a }
// TODO: figure out a 'newtype'-like way of avoiding actually creating new instances of this
public record Reader<R, A>(Func<R , A > RunReader) : K<Reader<R>, A> {

   public static A runReader(Reader<R, A> ma, R r) => ma.RunReader(r);

   // TODO: functions: ask, asks, local
   // ask :: Reader r r
   // ask = Reader id
   //
   // asks :: (r -> a) -> Reader r a
   // asks f = Reader f
   //
   // local :: (r -> r) -> Reader r a -> Reader r a
   // local f m = Reader $ \r -> runReader m (f r)
}


partial class Reader<R> : IFunctor<Reader<R>> {
   public static K<Reader<R>, B> FMap<A, B>(K<Reader<R>, A> fa, Func<A, B> func)
      => new Reader<R, B>(r => func(fa.As().RunReader(r)));
}


partial class Reader<R> : IApplicative<Reader<R>> {
   public static K<Reader<R>, A> Pure<A>(A a)
      => new Reader<R, A>(_ => a);


   public static K<Reader<R>, B> Apply<A, B>(K<Reader<R>, Func<A, B>> ffunc, K<Reader<R>, A> fa) 
   =>new Reader<R, B>(r=>ffunc(r()))=====
}


partial class Reader<R> : IMonad<Reader<R>> {
   public static K<Reader<R>, B> Bind<A, B>(K<Reader<R>, A> ma, Func<A, K<Reader<R>, B>> f)
      => new Reader<R, B>((R r) => Reader<R, A>.runReader(f(Reader<R, A>.runReader(ma)(r)))(r));
}



public static class ReaderExtensions {
   public static Reader<R, A> As<R, A>(this K<Reader<R>, A> ma) => (Reader<R, A>)ma;
}
