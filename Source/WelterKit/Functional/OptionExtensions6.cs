using WelterKit.Std.Functional;
using System;

namespace WelterKit.Functional;

public static class OptionExtensions6 {
   /// <summary>
   /// Adapts Option&lt;T&gt; to Either&lt;T, R&gt;
   /// </summary>
   /// <typeparam name="T">T is the new Left</typeparam>
   /// <typeparam name="R">R is the new Right</typeparam>
   public static Either<T, R> ToEitherLeft<T, R>(this Maybe<T> option, R right)
      => option is Some<T> some
               ? ( Left <T, R> )( T )some
               : ( Right<T, R> )right;


   /// <summary>
   /// Adapts Option&lt;T&gt; to Either&lt;T, R&gt;
   /// </summary>
   /// <typeparam name="T">T is the new Left</typeparam>
   /// <typeparam name="R">R is the new Right</typeparam>
   public static Either<T, R> ToEitherLeft<T, R>(this Maybe<T> option, Func<R> rightFunc)
      => option is Some<T> some
               ? ( Left <T, R> )( T )some
               : ( Right<T, R> )rightFunc();


   /// <summary>
   /// Adapts Option&lt;T&gt; to Either&lt;L, T&gt;
   /// </summary>
   /// <typeparam name="L">T is the new Left</typeparam>
   /// <typeparam name="T">T is the new Right</typeparam>
   public static Either<L, T> ToEitherRight<L, T>(this Maybe<T> option, Func<L> leftFunc)
      => option is Some<T> some
               ? ( Right<L, T> )( T )some
               : ( Left<L, T> )leftFunc();


   /// <summary>
   /// Adapts Option&lt;T&gt; to Either&lt;L, T&gt;
   /// </summary>
   /// <typeparam name="L">T is the new Left</typeparam>
   /// <typeparam name="T">T is the new Right</typeparam>
   public static Either<L, T> ToEitherRight<L, T>(this Maybe<T> option, L left)
      => option is Some<T> some
               ? ( Right<L, T> )( T )some
               : ( Left<L, T> )left;
}
