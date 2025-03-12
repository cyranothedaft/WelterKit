using System;
using System.Collections.Generic;



namespace WelterKit.Diagnostics {
   public abstract class ComplexDebugInfoBase<TObj> : DebugInfoBase<TObj> {
      private readonly DebugInfoLabelledItems _subItems;


      protected ComplexDebugInfoBase(TObj obj, Func<TObj, (string, DebugInfoBase)[]> labelledSubItemsFunc)
            : base(obj) {
         _subItems = ( obj != null )
                           ? new DebugInfoLabelledItems(labelledSubItemsFunc(obj))
                           : null;
      }


      protected override IEnumerable<DebugLine> RenderAsLines() {
         List<DebugLine> lines = new List<DebugLine>();
         string firstLineContents = GetFirstLineContents();
         if ( firstLineContents != null )
            lines.Add(new DebugLine(firstLineContents));
         if ( _subItems.Any() )
            lines.AddRange(_subItems.GetLines());
         return lines;
      }


      protected virtual string GetFirstLineContents() =>
            GetObjectTypePrefix();
   }
}
