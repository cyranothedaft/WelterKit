using System;
using System.Collections.Generic;



namespace WelterKit.Std.Functional {
   public sealed class None<T> : Maybe<T>,
                                 IEquatable<None<T>>,
                                 IEquatable<None>,
                                 IEquatable<Some<T>>

   {
      public override string ToString()
         => $"None<{typeof( T ).Name}>";


      public override string ToString(Func<T, string?> _)
         => ToString();


      #region Equality

      // Note:  All None types are equivalent to each other

      public override bool Equals(Maybe<T> other, IEqualityComparer<T> _) => other is None<T> || other is None;
      public bool Equals(None<T> other, IEqualityComparer<T> _) => !( other is null );

      bool IEquatable<None<T>>.Equals(None<T>? other) => !( other is null );
      bool IEquatable<None>.Equals(None? other) => !(other is null );
      bool IEquatable<Some<T>>.Equals(Some<T>? other) => false;

      public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is None<T> || obj is None;
      public override int GetHashCode() => 0;

      public static bool operator ==(None<T> left, None<T> right) => true;
      public static bool operator !=(None<T> left, None<T> right) => false;

      #endregion Equality
   }
}
