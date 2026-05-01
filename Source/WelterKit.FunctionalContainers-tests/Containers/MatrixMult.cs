using System;
using System.Numerics;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Monoid (and thus Semigroup) included just for testing purposes.
// Represents square matrices with the (notable non-commutative) multiplication operation
// Formally:  The set Mn(R) of n×n square matrices over a ring R, with matrix multiplication, is a semigroup.
//            Because \(M_n(\mathbb{R})\) contains the identity matrix \(I=\left[\begin{matrix}1&0\\ 0&1\end{matrix}\right]\), it is formally a monoid (a semigroup with an identity).


internal record MatrixMult<A>(Matrix<A> Values) : K<MatrixMult, A>;


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


internal partial class MatrixMult : ISemigroup<MatrixMult> {
   public static K<MatrixMult, A> Combine<A>(K<MatrixMult, A> a1, K<MatrixMult, A> a2)
      => new MatrixMult<A>(MatrixMath.Mult(a1.As().Values,
                                           a2.As().Values));
}



internal static class MatrixMultExtensions {
   public static MatrixMult<A> As<A>(this K<MatrixMult, A> ma) => (MatrixMult<A>)ma;
}


internal static class MatrixMath {
   internal static Matrix<T> Mult<T>(Matrix<T> a, Matrix<T> b) {
      if (a.ColCount != b.RowCount) throw new ArgumentException($"Matrix sizes ({nameof( a )}: {a.RowCount}x{a.ColCount}, {nameof( b )}: {b.RowCount}x{b.ColCount}) are not compatible for multiplying.");

      // TODO: a more functional approach
      T[,] result = new T[a.RowCount, b.ColCount];

      for (int i = 0; i < a.RowCount; ++i)
      for (int j = 0; j < b.ColCount; ++j) {
         var row = a.RowVector(i);
         var col = b.ColVector(j);
         var rv = Vector.Create(row);
         var cv = new Vector<T>(col);
         var dot = Vector.Dot(rv, cv);
         result[i, j] = dot;

         // result[i, j] = Vector.Dot(new Vector<T>(a.RowVector(i)),
         //                           new Vector<T>(b.ColVector(j)));
      }

      return new Matrix<T>(result);
   }
}
