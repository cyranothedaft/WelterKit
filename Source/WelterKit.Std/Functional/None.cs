using System;


namespace WelterKit.Std.Functional {
   public sealed class None : IEquatable<None> {
      public static None Value { get; } = new None();

      private None() { }

      public override string ToString()
         => "None";


      #region Equality

      public bool Equals(None other) => true;
      public override int GetHashCode() => 0;

      #endregion Equality
   }
}
