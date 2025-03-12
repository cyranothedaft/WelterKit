using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WelterKit.StaticUtilities {
   public static class StringBuilderUtil {
      public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string toAppend)
         => condition
                  ? sb.Append(toAppend)
                  : sb;


      // TODO: unit test and ensure func isn't evaluated if condition is false
      public static StringBuilder AppendIf(this StringBuilder sb, bool condition, Func<string> toAppendFunc)
         => condition
                  ? sb.Append(toAppendFunc())
                  : sb;


      public static StringBuilder AppendIf(this StringBuilder sb, bool condition, StringBuilder toAppend)
         => condition
                  ? sb.Append(toAppend)
                  : sb;


      // TODO: unit test and ensure func isn't evaluated if condition is false
      public static StringBuilder AppendIf(this StringBuilder sb, bool condition, Func<StringBuilder> toAppendFunc)
         => condition
                  ? sb.Append(toAppendFunc())
                  : sb;


      // public static StringBuilder AppendIf(this StringBuilder sb, bool condition, Func<StringBuilder, StringBuilder> toAppendChainFunc) { } //TODO?


      public static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<string> lines)
         => lines.Aggregate(sb, (current, line) => current.AppendLine(line));


      public static StringBuilder AppendLines(this StringBuilder sb, params string[] lines)
         => AppendLines(sb, ( IEnumerable<string> )lines);
   }
}
