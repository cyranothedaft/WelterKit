using System;



namespace WelterKit.Std.Diagnostics {
   public interface IDiagInfoGetter<T> {
      DebugInfoBase<T> Info(T obj);
   }
}
