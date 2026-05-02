using System;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Monoid included just for testing purposes.
// Represents square matrices with the (notable non-commutative) multiplication operation.


internal record SquareMatrixMult3<N>(Matrix<N> Value) : MatrixMult<N>(Value),
                                                        IMonoid<SquareMatrixMult3<N>>
      where N : INumber<N> {

   public static SquareMatrixMult3<N> Empty
      => MatrixMath.Identity<N>(3);


   public SquareMatrixMult3<N> Combine(SquareMatrixMult3<N> rhs)
      => new SquareMatrixMult3<N>(base.Combine(rhs).Value);
}
