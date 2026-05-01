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
