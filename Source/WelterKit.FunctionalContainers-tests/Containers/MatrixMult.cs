using System;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Semigroup included just for testing purposes.
// Represents matrices with the (notable non-commutative) multiplication operation.


internal partial record MatrixMult<N>(Matrix<N> Value) : ISemigroup<MatrixMult<N>>
      where N : INumber<N> {

   public MatrixMult<N> Combine(MatrixMult<N> rhs)
      => new(MatrixMath.Mult(this.Value,
                             rhs.Value));
}
