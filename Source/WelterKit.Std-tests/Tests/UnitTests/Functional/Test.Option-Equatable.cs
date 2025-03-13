using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Functional;



namespace WelterKit.Std_Tests.Tests.UnitTests.Functional {
   [TestClass]
   public class Test_Maybe_Equatable {
      [TestMethod]
      public void Equals_GenericNone() {
         Maybe<int> none1 = new None<int>(),
                     none2 = new None<int>(),
                     some = 42;
         Assert.IsTrue(none1.Equals(none1));
         Assert.IsTrue(none2.Equals(none2));
         Assert.IsTrue(none1.Equals(none2));
         Assert.IsTrue(none2.Equals(none1));
         Assert.IsFalse(none1.Equals(some));
         Assert.IsFalse(none2.Equals(some));
      }


      [TestMethod]
      public void Equals_None() {
         None none1 = None.Value,
              none2 = None.Value;

         Assert.IsTrue(none1.Equals(none1));
         Assert.IsTrue(none2.Equals(none2));
         Assert.IsTrue(none1.Equals(none2));
         Assert.IsTrue(none2.Equals(none1));
      }


      [TestMethod]
      public void Equals_Some() {
         Maybe<int> none = None.Value,
                     some1 = 42,
                     some2 = 42,
                     some3 = 123;
         Assert.IsTrue(some1.Equals(some1));
         Assert.IsTrue(some2.Equals(some2));
         Assert.IsTrue(some3.Equals(some3));
         Assert.IsTrue(some1.Equals(some2));
         Assert.IsTrue(some2.Equals(some1));
         Assert.IsFalse(some1.Equals(none));
         Assert.IsFalse(some2.Equals(none));
         Assert.IsFalse(some3.Equals(none));
      }


      [TestMethod]
      public void Equals_StringCaseInsensitive() {
         Maybe<string> some1 = "ABC",
                        some2 = "abc";
         Assert.IsTrue(some1.Equals(some2, StringComparer.OrdinalIgnoreCase));
      }


      [TestMethod]
      public void GetHashCode_GenericNone() {
         None<int> none = new None<int>();
         Assert.AreEqual(0, none.GetHashCode());

         Maybe<int> option = none;
         Assert.AreEqual(0, option.GetHashCode());
      }


      [TestMethod]
      public void GetHashCode_None() {
         None none = None.Value;
         Assert.AreEqual(0, none.GetHashCode());

         Maybe<int> option = none;
         Assert.AreEqual(0, option.GetHashCode());
      }


      [TestMethod]
      public void GetHashCode_Some_int() {
         int i1 =  42,
             i2 = 123;

         Maybe<int> option1 = i1,
                     option2 = i2;
         Assert.AreEqual( 42, option1.GetHashCode());
         Assert.AreEqual(123, option2.GetHashCode());

         Some<int> some1 = ( Some<int> )option1,
                   some2 = ( Some<int> )option2;
         Assert.AreEqual( 42, some1.GetHashCode());
         Assert.AreEqual(123, some2.GetHashCode());
      }


      [TestMethod]
      public void GetHashCode_Some_string() {
         string s0 = null,
                s1 = "",
                s2 = "abc",
                s3 = "xyz";

         Maybe<string> option0 = s0,
                        option1 = s1,
                        option2 = s2,
                        option3 = s3;
         Assert.AreEqual(0, option0.GetHashCode());
         Assert.AreEqual(s1.GetHashCode(), option1.GetHashCode());
         Assert.AreEqual(s2.GetHashCode(), option2.GetHashCode());
         Assert.AreEqual(s3.GetHashCode(), option3.GetHashCode());

         Some<string> some0 = ( Some<string> )option0,
                      some1 = ( Some<string> )option1,
                      some2 = ( Some<string> )option2,
                      some3 = ( Some<string> )option3;
         Assert.AreEqual(0, some0.GetHashCode());
         Assert.AreEqual(s1.GetHashCode(), some1.GetHashCode());
         Assert.AreEqual(s2.GetHashCode(), some2.GetHashCode());
         Assert.AreEqual(s3.GetHashCode(), some3.GetHashCode());
      }
   }
}
