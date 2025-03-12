using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace WelterKit.StaticUtilities {
   public static class StringUtil {

      // TODO: unit test
      public static IEnumerable<string> EnumLines(this string s) {
         using ( var reader = new StringReader(s) ) {
            string line;
            while ( ( line = reader.ReadLine() ) != null )
               yield return line;
         }
      }


      public static string JoinString(this IEnumerable<string> source, string delimiter)
         => string.Join(delimiter, source);


      public static string JoinString<T>(this IEnumerable<T> source, string delimiter, Func<T, string> format)
         => JoinString(source.Select(format), delimiter);


      public static string JoinString(this IEnumerable<string> source, char delimiter)
         => string.Join(delimiter, source);


      public static string JoinString<T>(this IEnumerable<T> source, char delimiter, Func<T, string> format)
         => JoinString(source.Select(format), delimiter);


      public static string OrBlank(this string s, Func<string, string> ifNotBlankFunc)
         => string.IsNullOrEmpty(s)
               ? ""
               : ifNotBlankFunc(s);


      // TODO: consider optimizing
      // TODO: consider making a SafeOverwrite version that doesn't throw exceptions
      /// <exception cref=""></exception>
      public static string Overwrite(this string s, string replacement, int startPos)
         => s[..startPos]
          + replacement
          + s.Substring(startPos + replacement.Length, s.Length - startPos - replacement.Length);


      public static string Repeat(this char c, int count)
         => new string(c, count);


      public static string Repeat(this string s, int count) {
         if ( s == null ) throw new ArgumentNullException(nameof( s ));
         if ( count < 0 ) throw new ArgumentOutOfRangeException(nameof( count ), $"count ({count}) cannot be less than zero.");
         return string.Concat(Enumerable.Repeat(s, count));
      }


      public static (string, string) SplitAt(this string s, int pos) {
         if ( s == null ) throw new ArgumentNullException(nameof( s ));
         if ( pos < 0 || pos > s.Length ) throw new ArgumentOutOfRangeException(nameof( pos ), $"pos ({pos}) cannot be less than zero or greater than s.Length.");
         return ( s.Substring(0, pos),
                  s.Substring(pos) );
         //return ( pos == 0        ? "" : s.Substring(0, pos),
         //         pos == s.Length ? "" : s.Substring(pos) );
      }


      public static (string, string) SplitForSuffix(this string fullString, string suffix) {
         if ( !fullString.EndsWith(suffix) )
            throw new InvalidOperationException($"\"{fullString}\" doesn't end with \"{suffix}\".");
         return fullString.SplitAt(fullString.Length - suffix.Length);
      }


      public static string? StripWhitespace(this string? s) {
         if ( s == null ) return null;
         char[] allWhitespaceChars = { ' ', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v' };
         char[] strippedChars = s.Where(c => !allWhitespaceChars.Contains(c))
                                 .ToArray();
         return new string(strippedChars);
      }


      public static string? RemoveStart(this string? s, string? start) {
         return s == null || string.IsNullOrEmpty(start) || !s.StartsWith(start)
                      ? s
                      : s.Substring(start.Length);
      }


      public static string? RemoveEnd(this string? s, string? end) {
         return s == null || string.IsNullOrEmpty(end) || !s.EndsWith(end)
                      ? s
                      : s.Substring(0, s.Length - end.Length);
      }


      public static string SurroundWith(this string s, string beforeAndAfter)
         => SurroundWith(s, beforeAndAfter, beforeAndAfter);

      public static string SurroundWith(this string s, char beforeAndAfter)
         => SurroundWith(s, beforeAndAfter, beforeAndAfter);

      public static string SurroundWith(this string s, char before, char after)
         => string.Concat(before, s, after);

      public static string SurroundWith(this string s, string before, string after)
         => string.Concat(before, s, after);


      public static string? Until(this string? s, string? until) {
         if ( s == null ) return null;
         if ( string.IsNullOrEmpty(until) ) return s;
         int pos = s.IndexOf(until, StringComparison.Ordinal);
         return ( pos >= 0 )
                      ? s.Substring(0, pos)
                      : s;
      }
   }
}
