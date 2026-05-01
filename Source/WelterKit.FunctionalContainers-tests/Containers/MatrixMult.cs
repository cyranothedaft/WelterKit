using System;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Semigroup included just for testing purposes.
// Represents matrices with the (notable non-commutative) multiplication operation.


internal partial record MatrixMultData<N>(Matrix<N> Value) : ISemigroup<MatrixMultData<N>> where N : INumber<N> {
   public MatrixMultData<N> Combine(MatrixMultData<N> rhs)
      => new(MatrixMath.Mult(this.Value,
                             rhs.Value));


   // MatrixMultData<N> ISemigroup<MatrixMultData<N>>.Combine(MatrixMultData<N> rhs)
   //    => new(MatrixMath.Mult(this.Value,
   //                           rhs.Value));
}
