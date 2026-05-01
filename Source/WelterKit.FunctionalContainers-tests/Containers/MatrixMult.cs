using System;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.Containers;

// A nontrivial example of a Monoid (and thus Semigroup) included just for testing purposes.
// Represents square matrices with the (notable non-commutative) multiplication operation
// Formally:  The set Mn(R) of n×n square matrices over a ring R, with matrix multiplication, is a semigroup.
//            Because \(M_n(\mathbb{R})\) contains the identity matrix \(I=\left[\begin{matrix}1&0\\ 0&1\end{matrix}\right]\), it is formally a monoid (a semigroup with an identity).


public record MatrixMult<A>(A[,] Values) : K<MatrixMult, A>;


// public class MatrixMult {
//    private readonly A[,] _a;
//    private readonly int _size;
//    
//    public MatrixMult(A[,] a) {
//       if (a.GetLength(0) != a.GetLength(1)) throw new ArgumentException(nameof( a ) + " is not a square matrix.", nameof( a ));
//       _a    = a;
//       _size = a.GetLength(0);
//    }
// }


public partial class MatrixMult : ISemigroup<MatrixMult> {
   public static K<MatrixMult, A> Combine<A>(K<MatrixMult, A> a1, K<MatrixMult, A> a2)
      => new MatrixMult<A>(MatrixMath.Mult(a1.As().Values,
                                           a2.As().Values));
}



public static class MatrixMultExtensions {
   public static MatrixMult<A> As<A>(this K<MatrixMult, A> ma) => (MatrixMult<A>)ma;
}



internal static class MatrixMath {
   internal static A[,] Mult<A>(A[,] a1, A[,] a2) {

   }

}
