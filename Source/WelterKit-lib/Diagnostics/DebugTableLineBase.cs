using System;
using System.Collections.Generic;



namespace WelterKit.Diagnostics {
   public abstract class DebugTableLineBase<TObj> : DebugInfoBase<TObj> {
      protected Dictionary<string, int> ColumnWidths { get; }


      protected DebugTableLineBase(TObj obj, Dictionary<string, int> columnWidths)
            : base(obj) {
         ColumnWidths = columnWidths;
      }


      protected abstract string GetContents();


      protected override IEnumerable<DebugLine> RenderAsLines() {
         yield return new DebugLine(GetContents());
      }


      protected override string RenderAsString(string outerIndent) =>
            GetContents();
   }
}

