using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests.Tests.UnitTests.Diagnostics {
   [TestClass]
   public class Test_ListDebugInfo {
      [TestMethod]
      public void ListDebugInfo_sample() {
         var list = new List<Test_DebugInfo.ClassForTables>()
                       {
                          new Test_DebugInfo.ClassForTables() { Col1 = "abc", Col2 = "aaa.bbb.ccc", Col3 = "123456789.123456789." },
                          new Test_DebugInfo.ClassForTables() { Col1 = "defdef", Col2 = "ddd.eee", Col3 = "123456789.123456789.123456789." },
                          new Test_DebugInfo.ClassForTables() { Col1 = "xyz", Col2 = "xyz", Col3 = "123456789.123456789." },
                       };
         var infoList = new ListDebugInfo<Test_DebugInfo.ClassForTables>(list, Test_DebugInfo.DiagTest.TableLine,
                                                                         null, null,
                                                                         ( "Col1", i => i.Col1 ),
                                                                         ( "Col2", i => i.Col2 ),
                                                                         ( "Col3", i => i.Col3 )); 
         string expected = "[cnt: 3] (List`1) System.Collections.Generic.List`1[WelterKit.Std_Tests.Tests.UnitTests.Diagnostics.Test_DebugInfo+ClassForTables]\r\n" +
                           "000: [abc   ] [aaa.bbb.ccc] [123456789.123456789.          ]\r\n" +
                           "001: [defdef] [ddd.eee    ] [123456789.123456789.123456789.]\r\n" +
                           "002: [xyz   ] [xyz        ] [123456789.123456789.          ]";
         string actual = infoList.ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void ListDebugInfo_empty() {
         var list = new List<Test_DebugInfo.ClassForTables>()
                       {
                       };
         var infoList = new ListDebugInfo<Test_DebugInfo.ClassForTables>(list, Test_DebugInfo.DiagTest.TableLine,
                                                                         null, null,
                                                                         ( "Col1", i => i.Col1 ),
                                                                         ( "Col2", i => i.Col2 ),
                                                                         ( "Col3", i => i.Col3 )); 
         string expected = "[cnt: 0] (List`1) System.Collections.Generic.List`1[WelterKit.Std_Tests.Tests.UnitTests.Diagnostics.Test_DebugInfo+ClassForTables]";
         string actual = infoList.ToString();
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void CalcMaxColWidths_sample() {
         var list = new List<Test_DebugInfo.ClassForTables>()
                       {
                          new Test_DebugInfo.ClassForTables() { Col1 = "abc", Col2 = "aaa.bbb.ccc", Col3 = "123456789.123456789." },
                          new Test_DebugInfo.ClassForTables() { Col1 = "defdef", Col2 = "ddd.eee", Col3 = "123456789.123456789.123456789." },
                          new Test_DebugInfo.ClassForTables() { Col1 = "xyz", Col2 = "xyz", Col3 = "123456789.123456789." },
                       };
         var selectors = new (string, Func<Test_DebugInfo.ClassForTables, string>)[]
                            {
                               ( "Col1", elem => elem.Col1 ),
                               ( "Col2", elem => elem.Col2 ),
                               ( "Col3", elem => elem.Col3 ),
                            };
         (string, int)[] widths_expected = new (string, int)[]
                                              {
                                                 ( "Col1", 6 ),
                                                 ( "Col2", 11 ),
                                                 ( "Col3", 30 ),
                                              };
         (string, int)[] widths_actual = ListDebugInfo<Test_DebugInfo.ClassForTables>
                                         .CalcMaxColWidths(list, selectors)
                                         .Select(kvp => ( kvp.Key, kvp.Value ))
                                         .ToArray();
         CollectionAssert.AreEqual(widths_expected, widths_actual);
      }


      [TestMethod]
      public void CalcMaxColWidths_empty() {
         var list = new List<Test_DebugInfo.ClassForTables>()
                       {
                       };
         var selectors = new (string, Func<Test_DebugInfo.ClassForTables, string>)[]
                            {
                               ( "Col1", elem => elem.Col1 ),
                               ( "Col2", elem => elem.Col2 ),
                               ( "Col3", elem => elem.Col3 ),
                            };
         (string, int)[] widths_expected = new (string, int)[]
                                              {
                                                 ( "Col1", 0 ),
                                                 ( "Col2", 0 ),
                                                 ( "Col3", 0 ),
                                              };
         (string, int)[] widths_actual = ListDebugInfo<Test_DebugInfo.ClassForTables>
                                         .CalcMaxColWidths(list, selectors)
                                         .Select(kvp => ( kvp.Key, kvp.Value ))
                                         .ToArray();
         CollectionAssert.AreEqual(widths_expected, widths_actual);
      }
   }
}
