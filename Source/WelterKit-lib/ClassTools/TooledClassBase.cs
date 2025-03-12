using System;
using System.Collections.Generic;



namespace WelterKit.ClassTools {

   // ideas for a more elegant and consistent comparison approach
   // like:
   // CompareByNullable((x,y) => CompareUtil.CompareEnum(x.OpType, y.OpType))
   //    .ThenBy((x,y) => CompareUtil.CompareEnum(x.TargetType, y.TargetType))
   //    ...
   // or:
   // virtual Func<x,y,int>[] GetCompareOps();


   public abstract class TooledClassBase {

      #region Comparison

      protected virtual IEnumerable<int> GetHashInts() { yield break; }

      // TODO?
      //public override bool Equals(object obj) => Equals(this, obj as T);

      public override int GetHashCode() {
         // using unchecked to explicitly ignore overflows.
         // (overflows are intended and desired here.)
         unchecked {
            const int prime1 = 127,
                      prime2 = 11;
            int hash = prime1;
            // TODO: ? consider using LINQ Aggregate() instead of foreach?
            foreach ( int hashInt in this.GetHashInts() )
               hash = hash * prime2 + hashInt;
            return hash;
         }
      }

      #endregion Comparison


      // TODO: add diagnostics?
   }
}
