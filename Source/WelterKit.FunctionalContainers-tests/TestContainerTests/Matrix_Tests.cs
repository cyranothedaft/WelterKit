using System;
using WelterKit.FunctionalContainers_tests.Containers;


namespace WelterKit.FunctionalContainers_tests.TestContainerTests;

[TestClass]
public class Matrix_Tests {
   [TestMethod]
   public void RowVector_Sample() {
      Matrix<int> m = new(new int[2, 3] { { 1, 2, 3 },
                                          { 4, 5, 6 } });
      CollectionAssert.AreEqual(new[] { 1, 2, 3 }, m.RowVector(0));
      CollectionAssert.AreEqual(new[] { 4, 5, 6 }, m.RowVector(1));
   }


   [TestMethod]
   public void ColVector_Sample() {
      Matrix<int> m = new(new int[2, 3] { { 1, 2, 3 },
                                          { 4, 5, 6 } });
      CollectionAssert.AreEqual(new[] { 1, 4 }, m.ColVector(0));
      CollectionAssert.AreEqual(new[] { 2, 5 }, m.ColVector(1));
      CollectionAssert.AreEqual(new[] { 3, 6 }, m.ColVector(2));
   }


   [TestMethod]
   public void Multiply_Empty() {
      Matrix<int> a = new(new int[0, 0]);
      Matrix<int> b = new(new int[0, 0]);
      Matrix<int> expected = new(new int[0, 0]);

      MatrixAssert.AreEqual(expected, MatrixMath.Mult(a, b));
   }


   [TestMethod]
   public void Multiply_Sample1() {
      Matrix<int> a = new(new int[2, 3] { { 1, 1, 1 },
                                          { 1, 1, 1 } });
      Matrix<int> b = new(new int[3, 4] { { 1, 1, 1, 1 },
                                          { 1, 1, 1, 1 },
                                          { 1, 1, 1, 1 } });
      Matrix<int> expected = new(new int[2, 4] { { 3, 3, 3, 3 },
                                                 { 3, 3, 3, 3 } });

      MatrixAssert.AreEqual(expected, MatrixMath.Mult(a, b));
   }
}



internal static class MatrixAssert {
   public static void AreEqual<T>(Matrix<T> expected, Matrix<T> actual) {
      Assert.AreEqual(expected.RowCount, actual.RowCount, "RowCount");
      Assert.AreEqual(expected.ColCount, actual.ColCount, "ColCount");
      for (int i = 0; i < expected.RowCount; ++i)
      for (int j = 0; j < expected.ColCount; ++j)
         Assert.AreEqual(expected.Array[i, j], actual.Array[i, j], $"[{i}, {j}]");
   }
}
