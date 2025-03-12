using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Functional;



namespace WelterKit_Tests.UnitTests.Functional {
   [TestClass]
   public class Test_Either_Equatable {
      [TestMethod]
      public void Equals_ReferenceMatch() {
         Either<Type1, Type2> either1 = new Type1(111),
                              either2 = new Type2(222);
         Assert.IsTrue(either1.Equals(either1));
         Assert.IsTrue(either2.Equals(either2));
      }


      [TestMethod]
      public void Equals_ReferenceMismatch_TypeMatch_ValueMatch() {
         Either<Type1, Type2> either1a = new Type1(111),
                              either1b = new Type1(111);
         Assert.IsTrue(either1a.Equals(either1b));
         Assert.IsTrue(either1b.Equals(either1a));
      }


      [TestMethod]
      public void Equals_ReferenceMismatch_TypeMatch_ValueMismatch() {
         Either<Type1, Type2> either1a = new Type1(111),
                              either1b = new Type1(9);
         Assert.IsFalse(either1a.Equals(either1b));
         Assert.IsFalse(either1b.Equals(either1a));
      }


      [TestMethod]
      public void Equals_ReferenceMismatch_TypeMismatch() {
         Either<Type1, Type2> either1 = new Type1(111),
                              either2 = new Type2(111);
         Assert.IsFalse(either1.Equals(either2));
         Assert.IsFalse(either2.Equals(either1));
      }


      [TestMethod]
      public void GetHashCodeEx() {
         var val1 = new Type1(111);
         var val2 = new Type2(222);
         Either<Type1, Type2> either1 = val1,
                              either2 = val2;
         Assert.AreEqual(val1.GetHashCode(), either1.GetHashCode());
         Assert.AreEqual(val2.GetHashCode(), either2.GetHashCode());
      }


      private struct Type1 : IEquatable<Type1> {
         public int Value { get; }

         public Type1(int value) {
            Value = value;
         }

         public bool Equals(Type1 other) => other.Value == this.Value;
         public override bool Equals(object obj) => obj is Type1 other && Equals(other);
         public override int GetHashCode() => Value;
      }


      private struct Type2 : IEquatable<Type2> {
         public int Value { get; }

         public Type2(int value) {
            Value = value;
         }

         public bool Equals(Type2 other) => other.Value == this.Value;
         public override bool Equals(object obj) => obj is Type2 other && Equals(other);
         public override int GetHashCode() => Value;
      }
   }
}
