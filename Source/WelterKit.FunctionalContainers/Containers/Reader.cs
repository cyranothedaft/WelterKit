using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers.Containers;

// newtype Reader r a = Reader { runReader :: r -> a }
// TODO: figure out a 'newtype'-like way of avoiding actually creating new instances of this
public record Reader<R, A>(
      // Unwraps the monad, requiring the environment input to produce the final value
      Func<R , A > RunReader
) : K<Reader<R>, A> {

   public static A runReader(Reader<R, A> ma) =>ma.RunReader(ma.)

   // TODO: functions: ask, asks, local
}


partial class Reader<R> : IFunctor<Reader<R>> {
}


partial class Reader<R> : IApplicative<Reader<R>> {
   public static K<Reader<R>, A> Pure<A>(A a)
      => new Reader<R, A>(_ => a);
}


partial class Reader<R> : IMonad<Reader<R>> {
   public static K<Reader<R>, B> Bind<A, B>(K<Reader<R>, A> ma, Func<A, K<Reader<R>, B>> f)
      => new Reader<R, B>((R r) => Reader<R, A>.runReader(f(Reader<R, A>.runReader(ma)(r)))(r));
}



public static class ReaderExtensions {
   public static Reader<R, A> As<R, A>(this K<Reader<R>, A> ma) => (Reader<R, A>)ma;
}
