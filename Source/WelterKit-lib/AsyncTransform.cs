using System;
using System.Threading.Tasks;



namespace WelterKit {
   /// <see href="https://docs.microsoft.com/en-us/archive/msdn-magazine/2015/july/async-programming-brownfield-async-development#transform-synchronous-to-asynchronous-code">Async Programming - Brownfield Async Development</see>
   public static class AsyncTransform {
      /// <summary>
      /// "The Blocking Hack"
      /// </summary>
      /// <param name="actionAsync">The innards of this action should end with .ConfigureAwait(false)</param>
      public static void InvokeAsSyncBlocking(this Func<Task> actionAsync) {
         actionAsync().GetAwaiter().GetResult();
      }


      /// <summary>
      /// "The Blocking Hack"
      /// </summary>
      /// <param name="funcAsync">The innards of this action should end with .ConfigureAwait(false)</param>
      public static TResult InvokeAsSyncBlocking<TResult>(this Func<Task<TResult>> funcAsync)
         => funcAsync().GetAwaiter().GetResult();


      /// <summary>
      /// Invokes a synchronous operation synchronously but still awaitable.
      /// i.e., the Task is returned after it has already completed.
      /// </summary>
      public static Task InvokeAsAsyncBlocking(this Action action) {
         action();
         return Task.CompletedTask;
      }


      /// <summary>
      /// Invokes a synchronous operation synchronously but still awaitable.
      /// i.e., the Task is returned after it has already completed.
      /// </summary>
      public static Task<TResult> InvokeAsAsyncBlocking<TResult>(this Func<TResult> func)
         => Task.FromResult(func());


      /// <summary>
      /// Invokes a synchronous operation asynchronously in a separate thread, via Task.Run().
      /// </summary>
      public static Task InvokeInThreadAsync(this Action action)
         => Task.Run(action);


      /// <summary>
      /// Invokes a synchronous operation asynchronously in a separate thread, via Task.Run().
      /// </summary>
      public static Task<TResult> InvokeInThreadAsync<TResult>(this Func<TResult> func)
         => Task.Run(func);
   }
}
