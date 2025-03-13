using System;
using System.Collections.Generic;



namespace WelterKit.Std.Functional {
   public sealed class Some<T> : Maybe<T>,
                                 IEquatable<Some<T>>,
                                 IEquatable<None<T>>,
                                 IEquatable<None> {
      private readonly T _value;


      public Some(T value) {
         _value = value;
      }


      public override string? ToString()
         => ToString(value => value?.ToString());


      public override string ToString(Func<T, string?> valueToString)
         => $"Some({valueToString(_value)})";


      /// <summary>
      /// Adapts Some&lt;T&gt; --&gt; T
      /// </summary>
      public static implicit operator T(Some<T> some)
         => some._value;


      #region Equality

      public static bool Equals(Some<T> x, Some<T> y) => Equals(x, y, EqualityComparer<T>.Default);
      public static bool Equals(Some<T> x, Some<T> y, IEqualityComparer<T> comparer) => comparer.Equals(x._value, y._value);

      public override bool Equals(Maybe<T> other, IEqualityComparer<T> valueComparer) => other is Some<T> some && Equals(this, some, valueComparer);
      public bool Equals(Some<T> other, IEqualityComparer<T> valueComparer) => valueComparer.Equals(this._value, other._value);

      bool IEquatable<Some<T>>.Equals(Some<T>? other) => !( other is null ) && Equals(this, other);
      bool IEquatable<None<T>>.Equals(None<T>? other) => false;
      bool IEquatable<None>.Equals(None? other) => false;

      public override bool Equals(object? obj) => ReferenceEquals(this, obj) || ( obj is Some<T> some && Equals(this, some) );

      public override int GetHashCode() => _value?.GetHashCode() ?? 0;

      public static bool operator ==(Some<T> left, Some<T> right) => Equals(left, right);
      public static bool operator !=(Some<T> left, Some<T> right) => !Equals(left, right);

      #endregion Equality
   }
}
