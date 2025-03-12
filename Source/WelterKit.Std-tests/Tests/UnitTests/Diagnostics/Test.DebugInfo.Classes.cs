using System;
using System.Collections.Generic;
using WelterKit.Std.Diagnostics;



namespace WelterKit_Tests.Tests.UnitTests.Diagnostics {
   partial class Test_DebugInfo {
      public partial class DiagTest {
         public static DebugInfoBase<InnerClass   > Info(InnerClass    obj) => InnerClass   .GetDebugInfo(obj);
         public static DebugInfoBase<OuterClass   > Info(OuterClass    obj) => OuterClass   .GetDebugInfo(obj);
         public static DebugInfoBase<ClassWithList> Info(ClassWithList obj) => ClassWithList.GetDebugInfo(obj);
         public static DebugInfoBase<ClassForTables> TableLine(ClassForTables obj, Dictionary<string, int> columnWidths) => ClassForTables.GetDebugTableLine(obj, columnWidths);
      }



      public class EmptyClass { }



      public class InnerClass {
         public bool Prop1;
         public string Property2;

         public static DebugInfoBase<InnerClass> GetDebugInfo(InnerClass obj) => new DebugInfo(obj);
         private class DebugInfo : ComplexDebugInfoBase<InnerClass> {
            public DebugInfo(InnerClass obj)
                  : base(obj, o => new (string, DebugInfoBase)[]
                                         {
                                               ( "Prop1", Diag.ObjectInfo(o.Prop1) ),
                                               ( "Property2", new StringInfo(o.Property2) ),
                                         }) {
            }
         }
      }



      public class OuterClass {
         public string Prop1 = "prop 1 value";
         public InnerClass Inner1 = new InnerClass();
         public InnerClass Inner2 = null;


         public static DebugInfoBase<OuterClass> GetDebugInfo(OuterClass obj) => new DebugInfo(obj);
         private class DebugInfo : ComplexDebugInfoBase<OuterClass> {
            public DebugInfo(OuterClass obj)
                  : base(obj, o => new (string, DebugInfoBase)[]
                                         {
                                               ( "Prop1", new StringInfo(o.Prop1) ),
                                               ( "Inner1", DiagTest.Info(o.Inner1) ),
                                               ( "Inner2", DiagTest.Info(o.Inner2) ),
                                         }) {
            }
         }
      }



      public class ClassWithList {
         public object OtherProp1;
         public List<string> ListProp = new List<string>()
                                              {
                                                    "",
                                                    "abc",
                                                    "xyz"
                                              };
         public object OtherProp2;

         public static DebugInfoBase<ClassWithList> GetDebugInfo(ClassWithList obj) => new DebugInfo(obj);
         private class DebugInfo : ComplexDebugInfoBase<ClassWithList> {
            public DebugInfo(ClassWithList obj)
                  : base(obj, o => new (string, DebugInfoBase)[]
                                         {
                                               ( "OtherProp1", Diag.ObjectInfo(o.OtherProp1) ),
                                               ( "ListProp", new StringListInfo(o.ListProp) ),
                                               ( "OtherProp2", Diag.ObjectInfo(o.OtherProp2) ),
                                         }) {
            }
         }
      }



      public class ClassForLiterals {
         public string AsLiteral;

         public static DebugInfoBase<ClassForLiterals> GetDebugInfo(ClassForLiterals obj) => new DebugInfo(obj);
         private class DebugInfo : ComplexDebugInfoBase<ClassForLiterals> {
            public DebugInfo(ClassForLiterals obj)
                  : base(obj, o => new (string, DebugInfoBase)[]
                                         {
                                               ( "AsLiteral", new LiteralInfo(o.AsLiteral) ),
                                               ( "Extra", new LiteralInfo("extra stuff here") ),
                                         }) {
            }
         }
      }



      public class ClassForTables {
         public string Col1, Col2, Col3;

         public static DebugInfoBase<ClassForTables> GetDebugTableLine(ClassForTables obj, Dictionary<string, int> columnWidths) => new DebugTableLine(obj, columnWidths);
         private class DebugTableLine : DebugTableLineBase<ClassForTables> {
            public DebugTableLine(ClassForTables obj, Dictionary<string, int> columnWidths) : base(obj, columnWidths) {
            }

            protected override string GetContents() =>
               string.Format("[{0,-" + ColumnWidths["Col1"] + "}] [{1,-" + ColumnWidths["Col2"] + "}] [{2,-" + ColumnWidths["Col3"] + "}]",
                             _obj.Col1,
                             _obj.Col2,
                             _obj.Col3);
         }
      }
   }
}
