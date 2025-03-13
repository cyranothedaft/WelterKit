using System;
using WelterKit.Std.Functional;


namespace WelterKit.Functional;

public static class EitherExtensions6 {
   public static Either<L, R> ToEitherFromNullable<L, R>(this R? nullable, Func<L> leftFunc)
         where R : class
      => nullable is not null
               ? nullable
               : leftFunc();
}
