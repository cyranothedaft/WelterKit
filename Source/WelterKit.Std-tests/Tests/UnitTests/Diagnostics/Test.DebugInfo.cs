using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.Diagnostics;



namespace WelterKit.Std_Tests.Tests.UnitTests.Diagnostics {
   [TestClass]
   public partial class Test_DebugInfo {
      [TestMethod]
      public void ObjectInfo_null() {
         var info = new ObjectInfo<object>(null);
         string expected = "[null]";
         string actual = info.ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ObjectInfo_1() {
         var info = new ObjectInfo<EmptyClass>(new EmptyClass());
         string expected = "(EmptyClass) WelterKit.Std_Tests.Tests.UnitTests.Diagnostics.Test_DebugInfo+EmptyClass";
         string actual = info.ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ObjectInfo_2() {
         var info = new ObjectInfo<InnerClass>(new InnerClass(), o => $"Prop1: {o.Prop1.ToString()}");
         string expected = "(InnerClass) Prop1: False";
         string actual = info.ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ToString_Complex1() {
         InnerClass inner = new InnerClass();
         string expected = @"(InnerClass)
Prop1    : (Boolean) False
Property2: [null]";
         string actual = DiagTest.Info(inner).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ToString_ComplexNested1() {
         OuterClass outer = new OuterClass();
         string expected = @"(OuterClass)
Prop1 : ""prop 1 value""
Inner1: (InnerClass)
        Prop1    : (Boolean) False
        Property2: [null]
Inner2: [null]";
         string actual = DiagTest.Info(outer).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void LiteralInfo_ToString() {
         Assert.AreEqual("[null]", new LiteralInfo(null).ToString());
         Assert.AreEqual("", new LiteralInfo("").ToString());
         Assert.AreEqual("abc xyz", new LiteralInfo("abc xyz").ToString());
      }


      [TestMethod]
      public void LiteralInfo_nested() {
         ClassForLiterals forLiterals = new ClassForLiterals() { AsLiteral = "abc xyz" };
         string expected = @"(ClassForLiterals)
AsLiteral: abc xyz
Extra    : extra stuff here";
         string actual = ClassForLiterals.GetDebugInfo(forLiterals).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Diag_ObjectInfo_NoFunc() {
         Assert.AreEqual("(Boolean) True", DiagStr.ObjectInfo(true));
      }


      [TestMethod]
      public void Diag_ObjectInfo_WithFunc() {
         Assert.AreEqual("(DateTime) 2/3/1 4:05:06",
                         DiagStr.ObjectInfo(new DateTime(1, 2, 3, 4, 5, 6), o => o.ToString("M/d/y h:mm:ss")));
      }
   }
}
