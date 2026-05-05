using System;

namespace WelterKit.FunctionalContainers.Framework.Traits;

public interface IMonoid<M> : ISemigroup<M> where M : IMonoid<M> {
   public static abstract M Empty { get; }
}
