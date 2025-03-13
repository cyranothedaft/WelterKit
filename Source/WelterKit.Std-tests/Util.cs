using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests {
   internal static class Util {
      internal static void AssertCollection<T>(ICollection<T> expected, ICollection<T> actual,
                                               Func<T, T, int> compareFunc, Func<T, string> toStringFunc = null,
                                               string desc = null) {
         if ( desc != null )
            Console.WriteLine("--- " + desc + " ---");
         Util.ShowList(expected, "expected", toStringFunc);
         Util.ShowList(actual  , "actual  ", toStringFunc);
         CollectionAssert.AreEqual(( ICollection )expected,
                                   ( ICollection )actual, new GeneralComparer<T>(compareFunc));
         Console.WriteLine("(match)");
      }


      internal static List<T> Seq<T>() => new List<T>();
      internal static List<T> Seq<T>(params T[] elements) => elements.ToList();


      internal static int IntCompare(int x, int y)
         => x - y;


      internal static void ShowList<T>(ICollection<T> list, string desc = null,
                                       Func<T, string> toStringFunc = null) {
         ShowList(( ICollection )list, desc, toStringFunc);
      }


      internal static void ShowList<T>(ICollection list, string desc = null,
                                       Func<T, string> toStringFunc = null) {
         Console.WriteLine("{0}: ({1}) {2}",
                           desc ?? "", list.Count,
                           string.Join(", ",
                                       list.Enumerate()
                                          .Cast<T>()
                                          .Select(toStringFunc ?? ( x => x.ToString() ))));
      }


      internal static IEnumerable<object> Enumerate(this ICollection collection) {
         foreach ( object obj in collection )
            yield return obj;
      }
   }
}
