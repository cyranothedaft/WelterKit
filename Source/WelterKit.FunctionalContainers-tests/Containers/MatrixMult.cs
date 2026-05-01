using System;
using System.Linq;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework.Kinds;


namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Monoid (and thus Semigroup) included just for testing purposes.
// Represents square matrices with the (notable non-commutative) multiplication operation
// Formally:  The set Mn(R) of n×n square matrices over a ring R, with matrix multiplication, is a semigroup.
//            Because \(M_n(\mathbb{R})\) contains the identity matrix \(I=\left[\begin{matrix}1&0\\ 0&1\end{matrix}\right]\), it is formally a monoid (a semigroup with an identity).



internal record MatrixMultData<N>(Matrix<N> Value) : ISemigroup<MatrixMultData<N>>
      where N : INumber<N> {
   MatrixMultData<N> ISemigroup<MatrixMultData<N>>.Combine(MatrixMultData<N> rhs)
      => new(MatrixMath.Mult(this.Value,
                             rhs.Value));
}

// internal record MatrixMultData(Matrix<double> Value) : K<MatrixMult, double>;


// public class MatrixMult {
//    private readonly Matrix<A> _a;
//    private readonly int _size;
//    
//    public MatrixMult(Matrix<A> a) {
//       if (a.GetLength(0) != a.GetLength(1)) throw new ArgumentException(nameof( a ) + " is not a square matrix.", nameof( a ));
//       _a    = a;
//       _size = a.GetLength(0);
//    }
// }


// internal partial class MatrixMult : ISemigroup<MatrixMult> {
//    // public static K<MatrixMult, double> Combine(K<MatrixMult, double> a1, K<MatrixMult, double> a2)
//    //    => new MatrixMultData(MatrixMath.Mult(a1.As().Value,
//    //                                         a2.As().Value));
//    public MatrixMult Combine(MatrixMult rhs) {
//       throw new NotImplementedException();
//    }
// }



// internal static class MatrixMultExtensions {
//    public static MatrixMultData As<A>(this K<MatrixMult, A> ma) => (MatrixMultData)ma;
// }


internal static class MatrixMath {
   internal static Matrix<N> Mult<N>(Matrix<N> a, Matrix<N> b) where N : INumber<N> {
      if (a.ColCount != b.RowCount) throw new ArgumentException($"Matrix sizes ({nameof( a )}: {a.RowCount}x{a.ColCount}, {nameof( b )}: {b.RowCount}x{b.ColCount}) are not compatible for multiplying.");

      // TODO: a more functional approach
      N[,] result = new N[a.RowCount, b.ColCount];

      for (int i = 0; i < a.RowCount; ++i)
      for (int j = 0; j < b.ColCount; ++j) {
         N[] row = a.RowVector(i);
         N[] col = b.ColVector(j);
         var dot = dotProduct(row, col);
         result[i, j] = dot;

         // result[i, j] = Vector.Dot(new Vector<T>(a.RowVector(i)),
         //                           new Vector<T>(b.ColVector(j)));
      }

      return new Matrix<N>(result);
   }


   private static N dotProduct<N>(N[] u, N[] v) where N : INumber<N>
      => Enumerable.Range(0, u.Length)
                   .Select(i => u[i] * v[i])
                   .Aggregate((x, y) => x + y);
}
