using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace WelterKit.Diagnostics {
   [DebuggerDisplay("({SafeTypeName()})")]
   public class InlineListDebugInfo<TElem> : SimpleDebugInfoBase<IList<TElem>> {
      private readonly Func<TElem, string> _elemToStringFunc;


      public InlineListDebugInfo(IList<TElem> list, Func<TElem, string> elemToStringFunc)
            : base(list) {
         _elemToStringFunc = elemToStringFunc;
      }


      protected override string GetContents() =>
            string.Format("[cnt: {0}] {1} {{ {2} }}",
                          _obj.Count,
                          GetObjectTypePrefix(),
                          string.Join(", ", _obj.Select(elem => _elemToStringFunc(elem)))
                         );
   }
}
