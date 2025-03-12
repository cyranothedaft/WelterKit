using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WelterKit.StaticUtilities;



namespace WelterKit.Diagnostics {
   partial class Diag {
      public static ListDebugInfo<FileSystemInfo> InfoLineList(IList<FileSystemInfo> list) => Diag.ObjectListInfo(list, info => info.FullName);

      public static InlineStringListDebugInfo StringListLine(IList<string> list) => new InlineStringListDebugInfo(list);

      //public static ListDebugInfo<FileSystemInfo> InfoLineList(IList<FileSystemInfo> list) => new ListDebugInfo<FileSystemInfo>(list, Diag.InfoLine);

      public static DebugInfoBase<FileSystemInfo> InfoLine(FileSystemInfo obj) => new DebugInfoLine_FileSystemInfo(obj);
      private class DebugInfoLine_FileSystemInfo : SimpleDebugInfoBase<FileSystemInfo> {
         public DebugInfoLine_FileSystemInfo(FileSystemInfo obj) : base(obj) { }
         protected override string GetContents() =>
               string.Format("{0}, LastWriteTimeUtc: {1}, FullName: {2}",
                             ( _obj is DirectoryInfo ) ? "Dir " : "File",
                             DiagStr.DateTimeInfo(_obj.LastWriteTimeUtc),
                             DiagStr.StringInfo(_obj.FullName));
      }


      public static ListDebugInfo<(string, string)> InfoLineList(IList<(string, string)> list) => Diag.ObjectListInfo(list, elem => string.Concat(DiagStr.StringInfo(elem.Item1), " -> ", DiagStr.StringInfo(elem.Item2)));
   }


   partial class DiagStr {
      public static string StringInfo(string str, bool surroundWithQuotes = true) => new StringInfo(str, surroundWithQuotes).ToString();

      public static string DateTimeInfo(DateTime dt) => new DateTimeInfo(dt).ToString();

      public static string StringListLine(IList<string> list) => Diag.StringListLine(list).ToString();
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class ObjectInfo<T> : SimpleDebugInfoBase<T> {
      private readonly Func<T, string> _contentsFunc;
      public ObjectInfo(T obj, Func<T, string> contentsFunc = null) : base(obj) { _contentsFunc = contentsFunc ?? ( o => o.ToString() ); }
      protected override string GetContents() => GetObjectTypePrefix() + " " + _contentsFunc(_obj);
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class StringInfo : SimpleDebugInfoBase<string> {
      private readonly bool _surroundWithQuotes;
      public StringInfo(string str, bool surroundWithQuotes = true) : base(str) { _surroundWithQuotes = surroundWithQuotes; }
      protected override string GetContents() => !( _obj is string s ) ? "[null]" : ( _surroundWithQuotes ? s.SurroundWith('"') : s );
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class DateTimeInfo : SimpleDebugInfoBase<DateTime> {
      public DateTimeInfo(DateTime dt) : base(dt) { }
      protected override string GetContents() => $"{_obj:O}[{_obj.Kind}]";
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class ObjectListInfo<T> : ListDebugInfo<T> {
      public ObjectListInfo(IList<T> list, Func<T, string> elemContentsFunc = null)
            : base(list, o => Diag.ObjectInfo(o, elemContentsFunc)) {
      }
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class StringListInfo : ListDebugInfo<string> {
      public StringListInfo(IList<string> list, bool surroundWithQuotes = true)
            : base(list, s => new StringInfo(s, surroundWithQuotes)) {
      }
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class InlineObjectListDebugInfo<T> : InlineListDebugInfo<T> {
      public InlineObjectListDebugInfo(IList<T> list) : base(list, o => o.ToString()) { }
      public InlineObjectListDebugInfo(IList<T> list, Func<T, string> toStringFunc) : base(list, toStringFunc) { }
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class InlineStringListDebugInfo : InlineListDebugInfo<string> {
      public InlineStringListDebugInfo(IList<string> list) : base(list, s => DiagStr.StringInfo(s)) { }
   }



   [DebuggerDisplay("({SafeTypeName()})")]
   public class DictionaryInfo<TKey, TValue> : ComplexDebugInfoBase<Dictionary<TKey, TValue>> {
      public DictionaryInfo(Dictionary<TKey, TValue> obj)
         : base(obj, o => new (string, DebugInfoBase)[]
                             {
                                ( "key-value pairs",
                                   new ObjectListInfo<KeyValuePair<TKey, TValue>>(o.ToList(), getPairInfo) )
                             }) {
      }


      // TODO: introduce TabularDebugInfoBase and use that instead of rendering to strings at this level
      private static string getPairInfo(KeyValuePair<TKey, TValue> kvp)
         => $"[{kvp.Key.ToString()}] -> {kvp.Value?.ToString() ?? "[null]"}";
   }
}
