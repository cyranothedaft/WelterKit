using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.StaticUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Testing {
   public static class SequenceAssert {
      public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual,
                                     Func<T, T, bool> equalsFunc = null, string message = null) {
         Func<T, T, bool> areEqual = equalsFunc ?? EqualityComparer<T>.Default.Equals;
         AreEqual<T, T>(expected, actual,
                        (expectedEl, actualEl, elMessage) => Assert.IsTrue(areEqual(expectedEl, actualEl), elMessage),
                        message);
      }


      public static void AreEqual<T, _>(IEnumerable<T> expected, IEnumerable<T> actual,
                                        string message = null, Func<T, T, string, _> assertAreEqualFunc = null)
         => AreEqual<T>(expected, actual, assertAreEqualAction: assertAreEqualFunc is null
                                                                      ? ( Action<T, T, string> )null
                                                                      : ( Action<T, T, string> )( (exp, act, msg) => assertAreEqualFunc(exp, act, msg) ),
                        message);


      public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual,
                                     Action<T, T, string> assertAreEqualAction = null, string message = null) {
         AreEqual<T, T>(expected, actual, assertAreEqualAction ?? Assert.AreEqual, message);
      }


      public static void AreEqual<TE, TA>(IEnumerable<TE> expected, IEnumerable<TA> actual,
                                          Action<TE, TA, string> assertAreEqual, string message = null) {
         message = !( message is null ) ? $"[{message}]" : string.Empty;

         IList<(int index, (TE expectedEl, TA actualEl, bool expectedEnded, bool actualEnded) pair)>
               indexedPairs = expected.ZipToEnd(actual)
                                      .Indexed()
                                      .ToList();

         int? expectedEndedAt = indexedPairs.Select(indexedPair => ( indexedPair.index, indexedPair.pair.expectedEnded ))
                                            .Cast<(int index, bool ended)?>()
                                            .FirstOrDefault(x => x.Value.ended)?.index;
         int? actualEndedAt = indexedPairs.Select(indexedPair => ( indexedPair.index, indexedPair.pair.actualEnded ))
                                          .Cast<(int index, bool ended)?>()
                                          .FirstOrDefault(x => x.Value.ended)?.index;
         if ( actualEndedAt.HasValue   ) Console.WriteLine($"{message} Warning - Actual sequence is too short (actual length:{actualEndedAt.Value}, expected length:{indexedPairs.Count}).");
         if ( expectedEndedAt.HasValue ) Console.WriteLine($"{message} Warning - Actual sequence is too long (actual length:{indexedPairs.Count}, expected length:{expectedEndedAt.Value}).");

         foreach ( ( int i, (TE expectedEl, TA actualEl, bool expectedEnded, bool actualEnded) pair ) in indexedPairs ) {
            if ( pair.expectedEnded ) {
               Assert.Fail($"Reached end of expected sequence at index {i}.", message);
               break;
            }
            else if ( pair.actualEnded ) {
               Assert.Fail($"Reached end of actual sequence at index {i}.", message);
               break;
            }

            assertAreEqual(pair.expectedEl, pair.actualEl,
                           $"{message} Elements at index {i} don't match:\r\n" +
                           $"Expected: {pair.expectedEl}\r\n"                  +
                           $"Actual:   {pair.actualEl}");
         }
      }
   }
}
