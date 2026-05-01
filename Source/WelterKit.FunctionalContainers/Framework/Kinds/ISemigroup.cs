using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface ISemigroup<M> where M : ISemigroup<M> {

   public M Combine(M rhs);

   // ===
   public static virtual M Combine(M a, M b) => a.Combine(b);

   //public static abstract K<M, A> Combine<A>(K<M, A> a1, K<M, A> a2);
}
