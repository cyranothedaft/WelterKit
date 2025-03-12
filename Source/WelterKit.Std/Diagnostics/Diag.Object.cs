using System;
using System.Collections.Generic;



namespace WelterKit.Std.Diagnostics {
   partial class Diag {
      public static ObjectInfo<T> ObjectInfo<T>(T obj) => new ObjectInfo<T>(obj);
      public static ObjectInfo<T> ObjectInfo<T>(T obj, Func<T, string> contentsFunc) => new ObjectInfo<T>(obj, contentsFunc);

      public static ObjectListInfo<T> ObjectListInfo<T>(IList<T> list) => new ObjectListInfo<T>(list);
      public static ObjectListInfo<T> ObjectListInfo<T>(IList<T> list, Func<T, string> elemContentsFunc) => new ObjectListInfo<T>(list, elemContentsFunc);

      public static InlineObjectListDebugInfo<T> ObjectListLine<T>(IList<T> list) => new InlineObjectListDebugInfo<T>(list);
      public static InlineObjectListDebugInfo<T> ObjectListLine<T>(IList<T> list, Func<T, string> toStringFunc) => new InlineObjectListDebugInfo<T>(list, toStringFunc);
   }


   partial class DiagStr {
      public static string ObjectInfo<T>(T obj) => Diag.ObjectInfo(obj).ToString();
      public static string ObjectInfo<T>(T obj, Func<T, string> contentsFunc) => Diag.ObjectInfo(obj, contentsFunc).ToString();

      public static string ObjectListInfo<T>(IList<T> list) => Diag.ObjectListInfo(list).ToString();
      public static string ObjectListInfo<T>(IList<T> list, Func<T, string> elemContentsFunc) => Diag.ObjectListInfo(list, elemContentsFunc).ToString();
   }
}
