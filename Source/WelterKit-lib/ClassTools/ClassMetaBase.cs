using System;
using WelterKit.Diagnostics;



namespace WelterKit.ClassTools {
   public abstract class ClassMetaBase {
   }


   public interface ISingleton<out T> {
      T Get { get; }
   }

   public interface IHasComparer<T> where T : TooledClassBase {
      GeneralComparatorBase<T> GetComparer();
   }
   public interface IHasDiagInfoGetter<T> {
      IDiagInfoGetter<T> GetDiagger();
   }
}
