using System;

namespace WelterKit.FunctionalContainers;

public static class Fn {
   public static Func<A, C> Compose<A, B, C>(Func<B, C> f, Func<A, B> g)
      => a => f(g(a));


   public static Func<A, Func<B,         TResult> > Curry<A, B   , TResult>(Func<A, B,    TResult> func) => a => b      => func(a, b);
   public static Func<A, Func<B, Func<C, TResult>>> Curry<A, B, C, TResult>(Func<A, B, C, TResult> func) => a => b => c => func(a, b, c);

}
