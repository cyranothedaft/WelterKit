using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace WelterKit.Diagnostics {
   /// <summary>
   /// A unified type for all DebugInfoBase typed variants
   /// </summary>
   public abstract class DebugInfoBase {
      public abstract DebugInfoLabelledItem LabelAsSubItem(string label);
   }


   [DebuggerDisplay("({SafeTypeName()})")]
   public abstract class DebugInfoBase<TObj> : DebugInfoBase {
      protected readonly TObj _obj;


      protected DebugInfoBase(TObj obj) {
         _obj = obj;
      }


      public override DebugInfoLabelledItem LabelAsSubItem(string label) {
         return new DebugInfoLabelledItem<TObj>(label, this);
      }


      public IEnumerable<DebugLine> SafeRenderAsLines() => IsNull(obj => RenderAsLines(), new[] { new DebugLine("[null]") });
      public string SafeRenderAsString(string outerIndent) => IsNull(obj => RenderAsString(outerIndent), "[null]");

      protected abstract IEnumerable<DebugLine> RenderAsLines();

      protected virtual string RenderAsString(string outerIndent) {
         return string.Join("\r\n", SafeRenderAsLines().Select(l => l.Indent(outerIndent).ToString()));
      }


      protected string GetObjectTypePrefix() => $"({SafeTypeName()})";


      protected string SafeTypeName() {
         return IsNull(obj => obj.GetType().Name, "null");
      }


      /// <summary>
      /// If our underlying object is null, do this with it, otherwise return that.
      /// </summary>
      protected TResult IsNull<TResult>(Func<TObj, TResult> func, TResult valueIfNull) {
         return ( _obj != null ) ? func(_obj) : valueIfNull;
      }


      public override string ToString() => SafeRenderAsString("");
   }
}
