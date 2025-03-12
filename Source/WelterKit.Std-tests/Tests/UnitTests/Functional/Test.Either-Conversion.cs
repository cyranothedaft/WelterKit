using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Functional;



namespace WelterKit_Tests.Tests.UnitTests.Functional {
   [TestClass]
   public class Test_Either_Conversion {
      private static readonly StructType OtherObject = new StructType();


      [TestMethod]
      public void Implicit_ToEither_FromL_int() {
         int obj = 42;
         Either<int, OtherType> either = obj;
         Assert.IsInstanceOfType(either, typeof( Left<int, OtherType> ));
         Assert.AreEqual(42, ( int )( Left<int, OtherType> )either);
      }


      [TestMethod]
      public void Implicit_ToEither_FromL_struct() {
         StructType obj = new StructType(42);
         Either<StructType, OtherType> either = obj;
         Assert.IsInstanceOfType(either, typeof( Left<StructType, OtherType> ));
         Assert.AreEqual(42, ( ( StructType )( Left<StructType, OtherType> )either ).Value);
      }


      [TestMethod]
      public void Implicit_ToEither_FromL_class() {
         ClassType obj = new ClassType(42);
         Either<ClassType, OtherType> either = obj;
         Assert.IsInstanceOfType(either, typeof( Left<ClassType, OtherType> ));
         Assert.AreEqual(42, ( ( ClassType )( Left<ClassType, OtherType> )either ).Value);
      }


      [TestMethod]
      public void Implicit_ToEither_FromL_interface() {
         IType val = new StructType(42);
         Either<IType, OtherType> either = val.ToEither<IType, OtherType>();
         Assert.IsInstanceOfType(either, typeof( Left<IType, OtherType> ));
         Assert.AreEqual(42, ( ( StructType )( Left<IType, OtherType> )either ).Value);
      }



      private struct OtherType { }

      private interface IType {
         int Value { get; }
      }

      private struct StructType : IType {
         public int Value { get; }
         public StructType(int value) { Value = value; }
      }

      private class ClassType : IType {
         public int Value { get; }
         public ClassType(int value) { Value = value; }
      }

   }
}
