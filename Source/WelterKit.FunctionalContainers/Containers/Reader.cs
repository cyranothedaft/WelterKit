using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers.Containers;

// newtype Reader r a = Reader { runReader :: r -> a }
// runReader :: Reader r a -> r -> a
public record Reader<R, A>(Func<R , A > RunReader) : K<Reader<R>, A> {

   public static A runReader(Reader<R, A> ma, R r) => ma.RunReader(r);

   // TODO: functions: ask, asks, local
   // ask :: m r 
   // asks :: (r -> r)
}


public partial class Reader<R>;


partial class Reader<R> : IFunctor<Reader<R>> {
   public static K<Reader<R>, B> FMap<A, B>(K<Reader<R>, A> fa, Func<A, B> func)
      => new Reader<R, B>(r => func(fa.As().RunReader(r)));
}


partial class Reader<R> : IApplicative<Reader<R>> {
   public static K<Reader<R>, A> Pure<A>(A a)
      => new Reader<R, A>(_ => a);


   // (<*>) :: (r -> a -> b) -> (r -> a) -> (r -> b)
   // (<*>) f g = \r -> (f r) (g r)
   public static K<Reader<R>, B> Apply<A, B>(K<Reader<R>, Func<A, B>> f, K<Reader<R>, A> g)
      => new Reader<R, B>(r => f.As().RunReader(r)
                                (g.As().RunReader(r)));
}


partial class Reader<R> : IMonad<Reader<R>> {
   // (>>=) :: Reader r a -> (a -> Reader r b) -> Reader r b
   // (Reader f1) >>= f2 = Reader $ \r -> runReader (f2 (f1 r)) r
   public static K<Reader<R>, B> Bind<A, B>(K<Reader<R>, A> rf1, Func<A, K<Reader<R>, B>> f2)
      => new Reader<R, B>(r => f2(rf1.As().RunReader(r)).As().RunReader(r));
}



public static class ReaderExtensions {
   public static Reader<R, A> As<R, A>(this K<Reader<R>, A> ma) => (Reader<R, A>)ma;
}
