using System;


namespace WelterKit.Std.Functional {
   public sealed class Unit {
      public static Unit Value = new Unit();

      private Unit() { }
   }
}
