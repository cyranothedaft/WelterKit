using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Functional;



namespace WelterKit_Tests.UnitTests.Functional {
   [TestClass]
   public class Test_MaybeExtensions {
      [TestMethod]
      public void IsSome_Sample() {
         Maybe<string> option = new Some<string>("abc");
         bool result = option.IsSome(out string value);
         Assert.IsTrue(result);
         Assert.AreEqual("abc", value);
      }


      [TestMethod]
      public void IsNone_Sample() {
         Maybe<string> option = None.Value;
         Assert.IsTrue(option.IsNone());
      }


      [TestMethod]
      public void Reduce_Some_Null() {
         Maybe<string> option = new Some<string>("abc");
         Assert.AreEqual("abc", option.Reduce(( string )null));
      }


      [TestMethod]
      public void Reduce_SomeValue() {
         Maybe<string> option = new Some<string>("abc");
         Assert.AreEqual("abc", option.Reduce(( string )null));
      }


      [TestMethod]
      public void Reduce_Func_SomeNull() {
         Maybe<string> option = new Some<string>(null);
         Assert.AreEqual(null, option.Reduce(() => "abc"));
      }


      [TestMethod]
      public void ToNullableValue_Some() {
         Maybe<int> option = new Some<int>(123);
         Assert.AreEqual(( int? )123, option.ToNullableValue());
      }


      [TestMethod]
      public void ToNullableValue_None() {
         Maybe<int> option = None.Value;
         Assert.AreEqual(( int? )null, option.ToNullableValue());
      }


      [TestMethod]
      public void ToNullable_SomeNull() {
         Maybe<string> option = new Some<string>(null);
         Assert.AreEqual(null, option.ToNullable());
      }


      #nullable enable
      [TestMethod]
      public void ToNullable_Some() {
         Maybe<string> option = new Some<string>("abc");
         Assert.AreEqual("abc", option.ToNullable());
      }


      [TestMethod]
      public void ToNullable_None() {
         Maybe<string> option = None.Value;
         Assert.AreEqual(( string? )null, option.ToNullable());
      }
      #nullable disable
   }
}
