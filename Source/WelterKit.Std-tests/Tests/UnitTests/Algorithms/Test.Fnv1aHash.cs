using System;
using WelterKit.Std.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WelterKit.Std_Tests.Tests.UnitTests.Algorithms {
   [TestClass]
   public class Test_Fnv1aHash {
      [TestMethod]
      public void Hash_Sample() {
         int hash1 = Fnv1aHash.Hash(10, 20, 30);
         int hash2 = Fnv1aHash.Hash(10, 20, 31);
         Assert.AreNotEqual(hash2, hash1);
      }
   }
}
