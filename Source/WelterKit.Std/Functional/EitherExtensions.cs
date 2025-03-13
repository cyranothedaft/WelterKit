using System;
using System.Threading.Tasks;


namespace WelterKit.Std.Functional {
   public static class EitherExtensions {
      public static bool IsLeft<L, R>(this Either<L, R> either)
         => either is Left<L, R>;


      public static bool IsLeft<L, R>(this Either<L, R> either, out L leftValue) {
         if ( either is Left<L, R> left ) {
            leftValue = left;
            return true;
         }
         else {
            leftValue = default( L );
            return false;
         }
      }


      public static bool IsRight<L, R>(this Either<L, R> either)
         => either is Right<L, R>;


      public static bool IsRight<L, R>(this Either<L, R> either, out R rightValue) {
         if ( either is Right<L, R> right ) {
            rightValue = right;
            return true;
         }
         else {
            rightValue = default( R );
            return false;
         }
      }


/*      Either<L,R>  ->      Either<L,RNew> , maps R ->               RNew                   */ public static            Either<L, RNew>  Map     <L, R, RNew>(this      Either<L, R>  either    , Func<R,                RNew  > map     ) => either is Right<L, R> right ? ( Either<L, RNew> )       map     (right)   : ( L )( Left<L, R> )either;
/* Task<Either<L,R>> -> Task<Either<L,RNew>>, maps R ->               RNew  , awaits arg     */ public static async Task<Either<L, RNew>> Map     <L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R,                RNew  > map     ) => Map(await eitherTask, map);
/*      Either<L,R>  -> Task<Either<L,RNew>>, maps R -> Task<         RNew> , awaits     map */ public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this      Either<L, R>  either    , Func<R, Task<          RNew> > mapAsync) => either is Right<L, R> right ? ( Either<L, RNew> )(await mapAsync(right))  : ( L )( Left<L, R> )either;
/*      Either<L,R>  ->      Either<L,RNew> , maps R ->      Either<L,RNew>                  */ public static            Either<L, RNew>  Map     <L, R, RNew>(this      Either<L, R>  either    , Func<R,      Either<L, RNew> > map     ) => either is Right<L, R> right ?                           map     (right)   : ( L )( Left<L, R> )either;
/* Task<Either<L,R>> -> Task<Either<L,RNew>>, maps R ->      Either<L,RNew> , awaits arg     */ public static async Task<Either<L, RNew>> Map     <L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R,      Either<L, RNew> > map     ) => Map(await eitherTask, map);
/*      Either<L,R>  -> Task<Either<L,RNew>>, maps R -> Task<Either<L,RNew>>, awaits     map */ public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this      Either<L, R>  either    , Func<R, Task<Either<L, RNew>>> mapAsync) => either is Right<L, R> right ?                    (await mapAsync(right) ) : ( L )( Left<L, R> )either;
/* Task<Either<L,R>> -> Task<Either<L,RNew>>, maps R -> Task<         RNew> , awaits arg     */ public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R, Task<          RNew> > mapAsync) => await MapAsync(await eitherTask, mapAsync);
/* Task<Either<L,R>> -> Task<Either<L,RNew>>, maps R -> Task<Either<L,RNew>>, awaits arg     */ public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R, Task<Either<L, RNew>>> mapAsync) => await MapAsync(await eitherTask, mapAsync);
                                                                                                public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R,      Either<L, RNew> > map     ) => (await eitherTask).Map(map);
                                                                                                public static async Task<Either<L, RNew>> MapAsync<L, R, RNew>(this Task<Either<L, R>> eitherTask, Func<R,                RNew  > map     ) => (await eitherTask).Map(map);




      public static Either<LNew, R> MapLeft<L, R, LNew>(this Either<L, R> either, Func<L, LNew> map)
         => either is Left<L, R> left
            ? ( Either<LNew, R> )map(left)
            : ( R )( Right<L, R> )either;


      public static async Task<Either<LNew, R>> MapLeft<L, R, LNew>(this Task<Either<L, R>> eitherTask, Func<L, LNew> map)
         => MapLeft(await eitherTask, map);


      public static R Reduce<L, R>(this Either<L, R> either, Func<L, R> map)
         => either is Left<L, R> left
            ? map(left)
            : ( Right<L, R> )either;


      public static async Task<R> ReduceAsync<L, R>(this Task<Either<L, R>> eitherTask, Func<L, R> reduceFunc)
         => (await eitherTask).Reduce(reduceFunc);


      /// <summary>
      /// Adapts L --&gt; Either&lt;L, R&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="Either&lt;L, R&gt;.From"/>
      public static Either<L, R> ToEither<L, R>(this L value)
         => new Left<L, R>(value);


      /// <summary>
      /// Adapts R --&gt; Either&lt;L, R&gt; explicitly.
      /// Useful when dealing with generic interfaces that don't allow for implicit conversion.
      /// </summary>
      /// <seealso cref="Either&lt;L, R&gt;.From"/>
      public static Either<L, R> ToEither<L, R>(this R value)
         => new Right<L, R>(value);


      /// <summary>
      /// Discards Right value if any.
      /// </summary>
      public static Maybe<L> ToMaybeFromLeft<L, R>(this Either<L, R> either)
         => either.IsLeft(out L leftValue)
                  ? ( Maybe<L> )leftValue
                  : ( Maybe<L> )None.Value;


      /// <summary>
      /// Discards Right value if any.
      /// </summary>
      public static async Task<Maybe<L>> ToMaybeFromLeftAsync<L, R>(this Task<Either<L, R>> eitherTask)
         => ToMaybeFromLeft(await eitherTask);


      /// <summary>
      /// Discards Left value if any.
      /// </summary>
      public static Maybe<R> ToMaybeFromRight<L, R>(this Either<L, R> either)
         => either.IsRight(out R rightValue)
                  ? ( Maybe<R> )rightValue
                  : ( Maybe<R> )None.Value;
   }
}
