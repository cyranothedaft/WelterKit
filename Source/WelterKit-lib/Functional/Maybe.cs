using System;
using System.Collections.Generic;



namespace WelterKit.Functional {
   public abstract class Maybe<T> { // TODO: interface-ize this so T can be marked contravariant?
      /// <summary>
      /// Adapts T --&gt; Maybe&lt;T&gt;
      /// </summary>
      public static implicit operator Maybe<T>(T value)
         => new Some<T>(value);


      /// <summary>
      /// Adapts None --&gt; Maybe&lt;T&gt;
      /// </summary>
      public static implicit operator Maybe<T>(None _)
         => new None<T>();


      /// <summary>
      /// Adapts T --&gt; Maybe&lt;T&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="MaybeExtensions.ToMaybe&lt;T&gt;"/>
      public static Maybe<T> From(T value)
         => new Some<T>(value);


      public abstract string ToString(Func<T, string?> valueToString);


      public abstract override bool Equals(object obj);
      public abstract bool Equals(Maybe<T> other, IEqualityComparer<T> valueComparer);
      public abstract override int GetHashCode();
   }
}
