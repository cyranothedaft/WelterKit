using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers.Containers;

// newtype Writer w a = Writer { runWriter :: (a, w) } 

// TODO: figure out a 'newtype'-like way of avoiding actually creating new instances of this
public record Writer<W, A>((W writer, A value) runWriter) : K<Writer<W>, A>
                                                          where W : IMonoid<W>;


public partial class Writer<W> where W : IMonoid<W>;


partial class Writer<W> : IFunctor<Writer<W>> {
   // fmap f (Writer (a, w)) = Writer (f a, w)
   public static K<Writer<W>, B> FMap<A, B>(K<Writer<W>, A> fa, Func<A, B> func) {
      (W w, A a) = fa.As().runWriter;
      return new Writer<W, B>((w, func(a)));
   }
}


partial class Writer<W> : IApplicative<Writer<W>> {
   // pure x = (mempty, x)
   public static K<Writer<W>, A> Pure<A>(A a)
      => new Writer<W, A>((W.Empty, a));


   // (Writer (logF, f)) <*> (Writer (logV, v)) = Writer (logF `mappend` logV, f v)
   public static K<Writer<W>, B> Apply<A, B>(K<Writer<W>, Func<A, B>> ffunc, K<Writer<W>, A> fa) {
      (W fw, Func<A, B> ff) = ffunc.As().runWriter;
      (W aw, A aa)          = fa   .As().runWriter;
      return new Writer<W, B>((fw.Combine(aw),
                               ff(aa)));
   }
}



public static class WriterExtensions {
   public static Writer<W, A> As<W, A>(this K<Writer<W>, A> ma) where W : IMonoid<W> => (Writer<W, A>)ma;
}
