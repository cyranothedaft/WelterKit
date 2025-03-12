using System;



namespace WelterKit.Diagnostics {
   public interface IDiagInfoGetter<T> {
      DebugInfoBase<T> Info(T obj);
   }
}
