using System;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Monoid included just for testing purposes.
// Represents square matrices with the (notable non-commutative) multiplication operation.


//internal abstract record SquareMatrixMultData<N>(Matrix<N> Value, int Size) : MatrixMultData<N>(Value),
//                                                                              IMonoid<SquareMatrixMultData<N>>
//      where N : INumber<N> {
//   // public static abstract SquareMatrixMultData<N> Empty { get; }
//   // public static SquareMatrixMultData<N> Empty { get; }
//
//   public SquareMatrixMultData<N> Combine(SquareMatrixMultData<N> rhs)
//      => new SquareMatrixMultData_1<N>(base.Combine(rhs).Value);
//}


internal record SquareMatrixMultData_3<N>(Matrix<N> Value) : MatrixMultData<N>(Value),
                                                                              IMonoid<SquareMatrixMultData_3<N>>
      where N : INumber<N> {

   public static SquareMatrixMultData_3<N> Empty
      => MatrixMath.Identity<N>(3);


   public SquareMatrixMultData_3<N> Combine(SquareMatrixMultData_3<N> rhs)
      => new SquareMatrixMultData_3<N>(base.Combine(rhs).Value);
}
