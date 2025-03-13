using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace WelterKit.Std.Diagnostics {
   [DebuggerDisplay("({SafeTypeName()})")]
   public class ListDebugInfo<TElem> : ComplexDebugInfoBase<IList<TElem>> {
      private readonly string? _altEmptyText;


      public ListDebugInfo(IList<TElem> list,
                           Func<TElem, DebugInfoBase<TElem>> elemToInfoFunc,
                           Func<int, string>? formatIndexFunc = null,
                           string? altEmptyText = null)
         : base(list, o => listToSubItems(o, elemToInfoFunc, formatIndexFunc ?? defaultFormatIndex)) {
         _altEmptyText = altEmptyText;
      }


      public ListDebugInfo(IList<TElem> list,
                           Func<TElem, Dictionary<string, int>, DebugInfoBase<TElem>> elemToInfoFunc,
                           Func<int, string>? formatIndexFunc = null,
                           string? altEmptyText = null,
                           params (string, Func<TElem, string>)[] variableColumnWidthSelectors)
         : base(list, o => listToSubItems(o, elemToInfoFunc, formatIndexFunc ?? defaultFormatIndex, CalcMaxColWidths(list, variableColumnWidthSelectors))) {
         _altEmptyText = altEmptyText;
      }


      private static (string, DebugInfoBase)[] listToSubItems(IList<TElem> list,
                                                              Func<TElem, DebugInfoBase> elemToInfoFunc,
                                                              Func<int, string> formatIndexFunc) {
         return list.Select((elem, idx) => ( formatIndexFunc(idx), elemToInfoFunc(elem) ))
                    .ToArray();
      }


      private static (string, DebugInfoBase)[] listToSubItems(IList<TElem> list,
                                                              Func<TElem, Dictionary<string, int>, DebugInfoBase> elemToInfoFunc,
                                                              Func<int, string> formatIndexFunc,
                                                              Dictionary<string, int> variableColumnWidths) {
         return list.Select((elem, idx) => ( formatIndexFunc(idx), elemToInfoFunc(elem, variableColumnWidths) ))
                    .ToArray();
      }


      protected override string GetFirstLineContents() =>
            ( _obj.Count > 0 || _altEmptyText == null )
                  ? $"[cnt: {_obj.Count}] {GetObjectTypePrefix()} {_obj.ToString()}"
                  : _altEmptyText;


      private static string defaultFormatIndex(int index) =>
            index.ToString("D3");


      internal static Dictionary<string, int> CalcMaxColWidths(IEnumerable<TElem> list, IReadOnlyList<(string, Func<TElem, string>)> columnSelectors) {
         if ( !columnSelectors.Any() ) throw new ArgumentException($"{nameof( columnSelectors )} list is empty.", nameof( columnSelectors ));

         Dictionary<string, int> columnWidths = columnSelectors.ToDictionary(sel => sel.Item1, sel => 0);
         foreach ( TElem elem in list ) {
            foreach ( ( string colName, Func<TElem, string> colSelectorFunc ) in columnSelectors ) {
               if ( !columnWidths.ContainsKey(colName) ) throw new KeyNotFoundException($"Column name \"{colName}\" missing from column widths for diagnostic output.");
               string str = colSelectorFunc(elem);
               int len = str?.Length ?? 0;
               if ( len > columnWidths[colName] )
                  columnWidths[colName] = len;
            }
         }
         return columnWidths;
      }
   }
}
