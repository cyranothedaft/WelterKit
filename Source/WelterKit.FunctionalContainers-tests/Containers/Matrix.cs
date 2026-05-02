using System;
using System.Linq;
using System.Numerics;



namespace WelterKit.FunctionalContainers_tests.Containers;


internal record Matrix<T>(T[,] Array) {
   public int RowCount => Array.GetLength(0);
   public int ColCount => Array.GetLength(1);


   public T[] RowVector(int r)
      => Enumerable.Range(0, ColCount)
                   .Select(c => {
                              Console.WriteLine($"# [{r}, {c}] of {RowCount}x{ColCount}]");
                              return Array[r, c];
                           })
                   .ToArray();


   public T[] ColVector(int c)
      => Enumerable.Range(0, RowCount)
                   .Select(r => Array[r, c])
                   .ToArray();
}


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


   public static SquareMatrixMult3<N> Identity<N>(int size) where N : INumber<N> {
      N[,] array = new N[3, 3];
      for (int i = 0; i < size; ++i)
      for (int j = 0; j < size; ++j)
         array[i, j] = (i == j ? N.One : N.Zero);
      return new SquareMatrixMult3<N>(new Matrix<N>(array));
   }
}
