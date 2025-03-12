using System;
using System.Collections.Generic;


namespace WelterKit.Functional {

   public abstract class Either<L, R> {
      public static implicit operator Either<L, R>(L obj) => new Left <L, R>(obj);
      public static implicit operator Either<L, R>(R obj) => new Right<L, R>(obj);

      public abstract override bool Equals(object obj);
      public abstract override int GetHashCode();


      /// <summary>
      /// Adapts L --&gt; Either&lt;L, R&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="EitherExtensions.ToEither&lt;L, R&gt;"/>
      public static Either<L, R> From(L value)
         => new Left<L, R>(value);


      /// <summary>
      /// Adapts R --&gt; Either&lt;L, R&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="EitherExtensions.ToEither&lt;L, R&gt;"/>
      public static Either<L, R> From(R value)
         => new Right<L, R>(value);
   }


   // TODO: allow nullable values



   public sealed class Left<L, R> : Either<L, R>,
                                    IEquatable<Left<L, R>>,
                                    IEquatable<Right<L, R>> {
      private readonly L _value;

      public Left(L value) {
         _value = value;
      }


      public override string? ToString()
         => _value?.ToString();


      public static implicit operator L(Left<L, R> obj)
         => obj._value;


      #region Equality

      bool IEquatable<Right<L, R>>.Equals(Right<L, R> other) => false;

      public static bool Equals(Left<L, R> x, Left<L, R> y) => EqualityComparer<L>.Default.Equals(x._value, y._value);


      public bool Equals(Left<L, R>? other) => !( other is null )
                                            && ( ReferenceEquals(this, other)
                                              || Equals(this, other) );
      public override bool Equals(object? obj) => ReferenceEquals(this, obj)
                                               || obj is Left<L, R> other
                                               && Equals(this, other);
      public override int GetHashCode() => _value?.GetHashCode() ?? 0;
      public static bool operator ==(Left<L, R> x, Left<L, R> y) => Equals(x, y);
      public static bool operator !=(Left<L, R> x, Left<L, R> y) => !Equals(x, y);

      #endregion Equality
   }



   public class Right<L, R> : Either<L, R>,
                              IEquatable<Right<L, R>>,
                              IEquatable<Left<L, R>> {
      private readonly R _value;

      public Right(R value) {
         _value = value;
      }


      public override string? ToString()
         => _value?.ToString();


      public static implicit operator R(Right<L, R> obj)
         => obj._value;


      #region Equality

      bool IEquatable<Left<L, R>>.Equals(Left<L, R> other) => false;

      public static bool Equals(Right<L, R> x, Right<L, R> y) => EqualityComparer<R>.Default.Equals(x._value, y._value);


      public bool Equals(Right<L, R>? other) => !( other is null )
                                             && ( ReferenceEquals(this, other)
                                               || Equals(this, other) );
      public override bool Equals(object? obj) => ReferenceEquals(this, obj)
                                               || obj is Right<L, R> other
                                               && Equals(this, other);
      public override int GetHashCode() => _value?.GetHashCode() ?? 0;
      public static bool operator ==(Right<L, R> x, Right<L, R> y) => Equals(x, y);
      public static bool operator !=(Right<L, R> x, Right<L, R> y) => !Equals(x, y);

      #endregion Equality
   }
}
