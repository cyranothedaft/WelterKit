using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Functional;



namespace WelterKit_Tests.Tests.UnitTests.Functional {
   [TestClass]
   public class Test_EitherExtensions {
      [TestMethod]
      public void IsRight_Sample() {
         Either<int, string> either = "abc";
         Assert.IsTrue(either.IsRight());
      }


      [TestMethod]
      public void MapLeft_Sample() {
         Either<int, OtherType> either = 123;
         Either<string, OtherType> mapped = either.MapLeft(val => val.ToString("X2"));
         Assert.AreEqual("7B", ( string )( Left<string, OtherType> )mapped);
      }



      private struct OtherType { }
   }
}
