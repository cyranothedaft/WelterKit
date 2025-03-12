using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace WelterKit.Diagnostics {
   [DebuggerDisplay("{_contents}")]
   public class DebugLine {
      private readonly string _contents;
      private readonly List<string> _indents = new List<string>();

      public DebugLine(string contents) {
         _contents = contents;
      }


      protected DebugLine(DebugLine source, string indentStr)
            : this(source._contents) {
         _indents.AddRange(source._indents);
         _indents.Add(indentStr);
      }


      public DebugLine Indent(int indent, char indentChar = ' ') {
         return Indent(new string(indentChar, indent));
      }


      public DebugLine Indent(string indentStr) {
         return string.IsNullOrEmpty(indentStr)
                      ? this
                      : AddIndent(indentStr);
      }


      protected virtual DebugLine AddIndent(string indentStr) =>
            new DebugLine(this, indentStr);


      public Labelled Label(string label, int siblingMaxLabelWidth) {
         return new Labelled(this, label, siblingMaxLabelWidth);
      }


      public override string ToString() {
         return string.Concat(_indents) + _contents;
      }



      [DebuggerDisplay("{_label}: {_contents}")]
      public class Labelled : DebugLine {
         private readonly string _label;
         private readonly int _siblingMaxLabelWidth;

         public char SpacingChar { get; set; } = ' ';

         public Labelled(DebugLine source, string label, int siblingMaxLabelWidth)
               : base(source._contents) {
            _label = label;
            _siblingMaxLabelWidth = siblingMaxLabelWidth;
         }


         protected Labelled(Labelled source, string indentStr)
               : base(source, indentStr) {
            _label = source._label;
            _siblingMaxLabelWidth = source._siblingMaxLabelWidth;
         }


         protected override DebugLine AddIndent(string indentStr) =>
               new Labelled(this, indentStr);


         public override string ToString() {
            return string.Concat(_indents) + $"{rpad(_label, _siblingMaxLabelWidth, SpacingChar)}: {_contents}";
         }


         private string rpad(string s, int padTo, char ch) {
            int strLen = s.Length;
            int padLen = ( padTo > strLen )
                               ? padTo - strLen
                               : 0;
            return s + new string(ch, padLen);
         }
      }
   }
}
