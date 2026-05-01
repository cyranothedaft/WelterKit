using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface ISemigroup<F> where F : ISemigroup<F> { 
   // ===
   public static abstract K<F, A> Combine<A>(K<F, A> a1, K<F, A> a2);
}
