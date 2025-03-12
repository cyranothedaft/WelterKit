using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.Std.BitConverters;


namespace WelterKit.Std.Algorithms {
   /// <summary>
   /// Implementation of the FNV-1a (Fowler–Noll–Vo) hash function.
   /// </summary>
   /// <remarks>This is NOT a cryptographic hash.</remarks>
   /// <seealso href="https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV-1a_hash" />
   public static class Fnv1aHash {
      public const uint FnvOffsetBasis = 0x811c9dc5;
      public const uint FnvPrime = 0x01000193;


      public static int Hash(params int[] codes) => Hash(( IEnumerable<int> )codes);

      // TODO: find reference test values and unit test this
      public static int Hash(IEnumerable<int> codes) {
         // using unchecked to explicitly ignore overflows.
         // (overflows are intended and desired here.)
         unchecked {
            return FourByteValue.ToInt(codes.Select(FourByteValue.ToUInt)
                                            .Aggregate(FnvOffsetBasis,
                                                       (current, t) => ( current ^ t ) * FnvPrime));
         }
      }


   }
}
