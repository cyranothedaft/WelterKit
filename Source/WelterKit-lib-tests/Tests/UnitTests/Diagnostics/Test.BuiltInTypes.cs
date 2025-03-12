using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Diagnostics;



namespace WelterKit_Tests.UnitTests.Diagnostics {
   [TestClass]
   public class Test_BuiltInTypes {
      [TestMethod]
      public void StringInfo() {
         Assert.AreEqual("[null]", new StringInfo(null).ToString());
         Assert.AreEqual("\"\"", new StringInfo("").ToString());
         Assert.AreEqual("\"abc xyz\"", new StringInfo("abc xyz").ToString());
         Assert.AreEqual("abc xyz", new StringInfo("abc xyz", surroundWithQuotes: false).ToString());
      }


      [TestMethod]
      public void DateTimeInfo() {
         Assert.AreEqual("0001-02-03T04:05:06.0000000Z[Utc]", new DateTimeInfo(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc)).ToString());
      }


      [TestMethod]
      public void StringListInfo_null() {
         string[] list = null;
         string expected = "[null]";
         string actual = new StringListInfo(list).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void StringListInfo_empty() {
         string[] list = new string[] { };
         string expected = "[cnt: 0] (String[]) System.String[]";
         string actual = new StringListInfo(list).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void StringListInfo_1() {
         string[] list = new string[]
                               {
                                     null,
                                     "",
                                     "a",
                                     "abc xyz"
                               };

         string expected = @"[cnt: 4] (String[]) System.String[]
000: [null]
001: """"
002: ""a""
003: ""abc xyz""";
         string actual = new StringListInfo(list).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void StringListInfo_NestedString1() {
         Test_DebugInfo.ClassWithList classWithList = new Test_DebugInfo.ClassWithList();
         string expected = @"(ClassWithList)
OtherProp1: [null]
ListProp  : [cnt: 3] (List`1) System.Collections.Generic.List`1[System.String]
            000: """"
            001: ""abc""
            002: ""xyz""
OtherProp2: [null]";
         string actual = Test_DebugInfo.DiagTest.Info(classWithList).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void StringListInfo_NestedObject1() {
         var list = new Test_DebugInfo.InnerClass[3]
                          {
                                new Test_DebugInfo.InnerClass(),
                                new Test_DebugInfo.InnerClass(),
                                new Test_DebugInfo.InnerClass(),
                          };
         string expected = @"[cnt: 3] (InnerClass[]) WelterKit_Tests.UnitTests.Diagnostics.Test_DebugInfo+InnerClass[]
000: (InnerClass)
     Prop1    : (Boolean) False
     Property2: [null]
001: (InnerClass)
     Prop1    : (Boolean) False
     Property2: [null]
002: (InnerClass)
     Prop1    : (Boolean) False
     Property2: [null]";
         string actual = new ListDebugInfo<Test_DebugInfo.InnerClass>(list, Test_DebugInfo.DiagTest.Info).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void StringListInfo_NestedObject2() {
         var list = new Test_DebugInfo.OuterClass[3]
                          {
                                new Test_DebugInfo.OuterClass(),
                                new Test_DebugInfo.OuterClass(),
                                new Test_DebugInfo.OuterClass(),
                          };
         string expected = @"[cnt: 3] (OuterClass[]) WelterKit_Tests.UnitTests.Diagnostics.Test_DebugInfo+OuterClass[]
000: (OuterClass)
     Prop1 : ""prop 1 value""
     Inner1: (InnerClass)
             Prop1    : (Boolean) False
             Property2: [null]
     Inner2: [null]
001: (OuterClass)
     Prop1 : ""prop 1 value""
     Inner1: (InnerClass)
             Prop1    : (Boolean) False
             Property2: [null]
     Inner2: [null]
002: (OuterClass)
     Prop1 : ""prop 1 value""
     Inner1: (InnerClass)
             Prop1    : (Boolean) False
             Property2: [null]
     Inner2: [null]";
         string actual = new ListDebugInfo<Test_DebugInfo.OuterClass>(list, Test_DebugInfo.DiagTest.Info).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Diag_StringListLine_1() {
         string expected = "[cnt: 4] (String[]) { [null], \"\", \"a\", \"abc xyz\" }";
         string actual = DiagStr.StringListLine(new string[] { null, "", "a", "abc xyz" });
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void DictionaryInfo_SampleStringInt() {
         var dic = new Dictionary<string, int>
                      {
                         { "1", 1 },
                         { "22", 22 },
                         { "333", 333 }
                      };

         string expected = @"(Dictionary`2)
key-value pairs: [cnt: 3] (List`1) System.Collections.Generic.List`1[System.Collections.Generic.KeyValuePair`2[System.String,System.Int32]]
                 000: (KeyValuePair`2) [1] -> 1
                 001: (KeyValuePair`2) [22] -> 22
                 002: (KeyValuePair`2) [333] -> 333";
         string actual = new DictionaryInfo<string, int>(dic).ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void DictionaryInfo_SampleIntString() {
         var dic = new Dictionary<int, string>
                      {
                         { 0, null },
                         { 1, "1" },
                         { 22, "22" },
                         { 333, "333" }
                      };

         string expected = @"(Dictionary`2)
key-value pairs: [cnt: 4] (List`1) System.Collections.Generic.List`1[System.Collections.Generic.KeyValuePair`2[System.Int32,System.String]]
                 000: (KeyValuePair`2) [0] -> [null]
                 001: (KeyValuePair`2) [1] -> 1
                 002: (KeyValuePair`2) [22] -> 22
                 003: (KeyValuePair`2) [333] -> 333";
         string actual = new DictionaryInfo<int, string>(dic).ToString();
         Assert.AreEqual(expected, actual);
      }
   }
}
