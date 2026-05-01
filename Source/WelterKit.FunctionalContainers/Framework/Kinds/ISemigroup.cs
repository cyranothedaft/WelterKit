using System;

namespace WelterKit.FunctionalContainers.Framework.Kinds;

public interface ISemigroup<M> where M : ISemigroup<M> {

   public M Combine(M rhs);

   // ===
   public static virtual M Combine(M a, M b) => a.Combine(b);
}
