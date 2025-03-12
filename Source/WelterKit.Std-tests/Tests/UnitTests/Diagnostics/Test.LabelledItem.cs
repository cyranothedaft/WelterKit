using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Diagnostics;



namespace WelterKit_Tests.Tests.UnitTests.Diagnostics {
   [TestClass]
   public class Test_LabelledItem {
      [TestMethod]
      public void Diag_Label_1() {
         Assert.AreEqual("label: [null]", DiagStr.Label("label", new ObjectInfo<object>(null)));
         Assert.AreEqual("label: \"abc xyz\"", DiagStr.Label("label", new StringInfo("abc xyz")));
      }


      [TestMethod]
      public void Diag_Label_2() {
         string expected = @"label: (InnerClass)
       Prop1    : (Boolean) False
       Property2: [null]";
         string actual = DiagStr.Label("label", Test_DebugInfo.DiagTest.Info(new Test_DebugInfo.InnerClass()));
         Assert.AreEqual(expected, actual);
      }
   }
}
