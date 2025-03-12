using System;


namespace WelterKit.Functional {
   public sealed class Unit {
      public static Unit Value = new Unit();

      private Unit() { }
   }
}
