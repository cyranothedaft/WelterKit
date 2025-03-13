using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace WelterKit.Std.Diagnostics {
   public class LabelledItem<T> {
      private readonly string _label;
      private readonly DebugInfoBase<T> _item;
      private readonly int? _indentOverride;


      public LabelledItem(string label, DebugInfoBase<T> item, int? indentOverride = null) {
         _label = label;
         _item = item;
         _indentOverride = indentOverride;
      }


      private IEnumerable<DebugLine> getLines() {
         DebugLine[] contentLines = _item.SafeRenderAsLines().ToArray();
         int indent = _indentOverride ?? _label.Length;
         yield return contentLines[0].Label(_label, indent);
         for ( int i = 1; i < contentLines.Length; ++i )
            yield return contentLines[i].Indent(indent + 2);
      }


      public string RenderAsString(string indent) {
         return string.Join("\r\n", getLines().Select(l => l.Indent(indent).ToString()));
      }


      public override string ToString() {
         return RenderAsString("");
      }
   }

   public abstract class DebugInfoLabelledItem {
      public string Label { get; }

      protected DebugInfoLabelledItem(string label) {
         Label = label;
      }

      public abstract IEnumerable<DebugLine> GetLines(int siblingMaxLabelWidth);
   }



   [DebuggerDisplay("{Label}: ({Contents.SafeTypeName()})")]
   public class DebugInfoLabelledItem<TObj> : DebugInfoLabelledItem {
      public DebugInfoBase<TObj> Contents { get; }


      public DebugInfoLabelledItem(string label, DebugInfoBase<TObj> contents)
            : base(label) {
         Contents = contents;
      }


      public override IEnumerable<DebugLine> GetLines(int siblingMaxLabelWidth) {
         DebugLine[] contentLines = Contents.SafeRenderAsLines().ToArray();
         yield return contentLines[0].Label(Label, siblingMaxLabelWidth);
         for ( int i = 1; i < contentLines.Length; ++i )
            yield return contentLines[i].Indent(siblingMaxLabelWidth + 2);
      }
   }


   public class DebugInfoLabelledItems {
      private readonly DebugInfoLabelledItem[] _items;


      public DebugInfoLabelledItems(IEnumerable<( string, DebugInfoBase)> labelledItems) {
         _items = labelledItems.Select(i => i.Item2.LabelAsSubItem(i.Item1))
                               .ToArray();
      }


      public bool Any() => _items.Any();


      public List<DebugLine> GetLines() {
         List<DebugLine> lines = new List<DebugLine>();
         int maxLabelWidth = _items.Max(i => i.Label.Length);
         foreach ( DebugInfoLabelledItem item in _items )
            lines.AddRange(item.GetLines(maxLabelWidth));
         return lines;
      }


      public string RenderAsString(string indent) {
         return string.Join("\r\n", GetLines().Select(l => l.Indent(indent).ToString()));
      }
   }



   partial class Diag {
   }
   
   partial class DiagStr {
      public static string Label<TData>(string label, DebugInfoBase<TData> objInfo, int? indentOverride = null) =>
            new LabelledItem<TData>(label, objInfo, indentOverride).ToString();
   }
}
