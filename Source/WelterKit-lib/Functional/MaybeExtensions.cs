using System;


namespace WelterKit.Functional {
   public static class MaybeExtensions {
      public static bool IsNone<T>(this Maybe<T> option)
         => option is None<T>;


      public static bool IsSome<T>(this Maybe<T> option)
         => option is Some<T>;


      public static bool IsSome<T>(this Maybe<T> option, out T value) {
         if ( option is Some<T> some ) {
            value = ( T )some;
            return true;
         }
         else {
            value = default( T )!;
            return false;
         }
      }


      /// <summary>
      /// Adapts Maybe&lt;T&gt; --&gt; Maybe&lt;TResult&gt;
      /// </summary>
      public static Maybe<TResult> Map<T, TResult>(this Maybe<T> option, Func<T, TResult> map)
         => option is Some<T> some
                  ? ( Maybe<TResult> )map(some)
                  : None.Value;


      public static Maybe<TResult> Map<T, TResult>(this Maybe<T> option, Func<T, Maybe<TResult>> map)
         => option is Some<T> some
                  ? map(some)
                  : None.Value;


      public static Maybe<T> MapThrough<T>(this Maybe<T> option, Action<T> action) {
         if ( option.IsSome(out T value) )
            action(value);
         return option;
      }


      public static void Map<T>(this Maybe<T> option, Action<T> action) {
         MapThrough(option, action);
      }


      /// <summary>
      /// Adapts (Maybe&lt;T&gt;, T) --&gt; T
      /// </summary>
      public static T Reduce<T>(this Maybe<T> option, T whenNone)
         => option is Some<T> some
            ? ( T )some
            : whenNone;


      /// <summary>
      /// Adapts (Maybe&lt;T&gt;, T) --&gt; T  (lazy variant)
      /// </summary>
      public static T Reduce<T>(this Maybe<T> option, Func<T> whenNone)
         => option is Some<T> some
            ? ( T )some
            : whenNone();


      public static Maybe<T> Reduce<T>(this Maybe<T> option, Maybe<T> ifNone)
         => option as Some<T> 
         ?? ifNone;


      public static T? ToNullable<T>(this Maybe<T> option) where T : class
         => option.Map(x => ( T? )x)
            .Reduce(( T? )null);


      public static T? ToNullableValue<T>(this Maybe<T> option) where T : struct
         => option.Map(x => ( T? )x)
            .Reduce(( T? )null);


      public static Maybe<T> ToMaybeFromNullable<T>(this T? obj) where T : class
         => !( obj is null )
                  ? ( Maybe<T> )obj
                  : ( Maybe<T> )None.Value;


      public static Maybe<T> ToMaybeFromNullableValue<T>(this T? value) where T : struct
         => value.HasValue
                  ? ( Maybe<T> )value.Value
                  : ( Maybe<T> )None.Value;


      /// <summary>
      /// Adapts T --&gt; Maybe&lt;T&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="Maybe&lt;T&gt;.From"/>
      public static Maybe<T> ToMaybe<T>(this T value)
         => new Some<T>(value);


      /// <summary>
      /// Adapts T --&gt; Maybe&lt;T&gt;
      /// </summary>
      public static Maybe<T> When<T>(this T value, Func<T, bool> predicate)
         => predicate(value)
                  ? ( Maybe<T> )value
                  : None.Value;
   }
}
