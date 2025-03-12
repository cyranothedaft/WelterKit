using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace WelterKit.Diagnostics {
   public abstract class SimpleDebugInfoBase<TObj> : DebugInfoBase<TObj> {
      protected SimpleDebugInfoBase(TObj obj)
            : base(obj) {
      }


      protected abstract string GetContents();


      protected override IEnumerable<DebugLine> RenderAsLines() {
         yield return new DebugLine(GetContents());
      }


      protected override string RenderAsString(string outerIndent) =>
            GetContents();
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class LiteralInfo : SimpleDebugInfoBase<string> {
      public LiteralInfo(string contents) : base(contents) { }
      protected override string GetContents() => _obj;
   }
}
