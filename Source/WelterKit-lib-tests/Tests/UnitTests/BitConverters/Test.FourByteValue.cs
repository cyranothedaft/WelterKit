﻿using System;
using WelterKit.BitConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WelterKit_Tests.UnitTests.BitConverters {
   [TestClass]
   public class Test_FourByteValue {
      [TestMethod]
      public void ToUInt_Sample() {
         const int intValue = -1;
         const uint expectedUintValue = 0xffffffff;
         Assert.AreEqual(expectedUintValue, FourByteValue.ToUInt(intValue));
      }


      [TestMethod]
      public void UIntToUInt_Sample() {
         const uint uintValue = 0xffffffff;
         const int expectedIntValue = -1;
         Assert.AreEqual(expectedIntValue, FourByteValue.ToInt(uintValue));
      }
   }
}
