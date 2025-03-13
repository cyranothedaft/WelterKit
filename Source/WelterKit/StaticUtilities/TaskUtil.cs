using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WelterKit.StaticUtilities;


public static class TaskUtil {
   /// <summary>
   /// TODO
   /// </summary>
   public static async Task PollAsync(this Func<Task> pollActionAsync, TimeSpan interval,
                                      CancellationToken cancellationToken, IProgress<bool>? progress, ILogger? logger = null) {
      logger?.LogTrace("> PollAsync (interval:{interval})", interval);
      int counter = 0;
      while ( !cancellationToken.IsCancellationRequested ) {
         logger?.LogTrace("> PollAsync [{counter}] action", counter);
         await pollActionAsync();
         logger?.LogTrace("< PollAsync [{counter}] action", counter);

         logger?.LogTrace("> PollAsync [{counter}] delay", counter);
         await Task.Delay(interval, cancellationToken);
         logger?.LogTrace("< PollAsync [{counter}] delay", counter);

         progress?.Report(true);
         ++counter;
      }
      logger?.LogTrace("< PollAsync - final counter: {counter}", counter);
   }


   /// <summary>
   /// TODO
   /// </summary>
   // TODO: consolidate common pieces with same version that takes async function
   public static async Task PollAsync(this Func<bool> pollToProceedFunc, TimeSpan interval,
                                      CancellationToken? cancellationToken = null, IProgress<bool>? progress = null, ILogger? logger = null) {
      logger?.LogTrace("> PollAsync (interval:{interval})", interval);
      bool isCancelled() => cancellationToken?.IsCancellationRequested ?? false;
      bool proceed = true;
      int counter = 0;
      while ( proceed && !isCancelled() ) {
         logger?.LogTrace("> PollAsync [{counter}] action", counter);
         proceed = pollToProceedFunc();
         logger?.LogTrace("< PollAsync [{counter}] action - (proceed:{proceed})", counter, proceed);

         if ( proceed ) {
            logger?.LogTrace("> PollAsync [{counter}] delay", counter);
            await Task.Delay(interval, cancellationToken ?? default);
            logger?.LogTrace("< PollAsync [{counter}] delay", counter);

            progress?.Report(true);
         }
         ++counter;
      }
      logger?.LogTrace("< PollAsync - final counter: {counter}", counter);
   }


   /// <summary>
   /// TODO
   /// </summary>
   public static async Task PollAsync(this Func<Task<bool>> pollToProceedFuncAsync, TimeSpan interval,
                                      CancellationToken? cancellationToken = null, IProgress<bool>? progress = null, ILogger? logger = null) {
      logger?.LogTrace("> PollAsync (interval:{interval})", interval);
      bool isCancelled() => cancellationToken?.IsCancellationRequested ?? false;
      bool proceed = true;
      int counter = 0;
      while ( proceed && !isCancelled() ) {
         logger?.LogTrace("> PollAsync [{counter}] action", counter);
         proceed = await pollToProceedFuncAsync();
         logger?.LogTrace("< PollAsync [{counter}] action - (proceed:{proceed})", counter, proceed);

         if ( proceed ) {
            logger?.LogTrace("> PollAsync [{counter}] delay", counter);
            await Task.Delay(interval, cancellationToken ?? default);
            logger?.LogTrace("< PollAsync [{counter}] delay", counter);

            progress?.Report(true);
         }
         ++counter;
      }
      logger?.LogTrace("< PollAsync - final counter: {counter}", counter);
   }


   /// <summary>
   /// TODO
   /// </summary>
   public static async Task<T> PollAsync<T>(this Func<CancellationToken?, Task<T>> pollFuncAsync,
                                            Func<T, bool> stopConditionFunc, TimeSpan interval,
                                            CancellationToken? cancellationToken = null, IProgress<bool>? progress = null, ILogger? logger = null) {
      logger?.LogTrace("> PollAsync (interval:{interval})", interval);
      bool isCancelled() => cancellationToken?.IsCancellationRequested ?? false;
      T state = default( T );
      int counter = 0;
      try {
         while ( !isCancelled() ) {
            logger?.LogTrace("> PollAsync [{counter}] action", counter);
            state = await pollFuncAsync(cancellationToken);
            logger?.LogTrace("< PollAsync [{counter}] action - (state:{state})", counter, state );

            logger?.LogTrace("> PollAsync [{counter}] stop condition", counter);
            bool stop = stopConditionFunc(state);
            logger?.LogTrace("< PollAsync [{counter}] stop condition (stop:{stop})", counter, stop);

            if ( stop || isCancelled() ) break;

            logger?.LogTrace("> PollAsync [{counter}] delay", counter);
            await Task.Delay(interval, cancellationToken ?? default);
            logger?.LogTrace("< PollAsync [{counter}] delay", counter);

            progress?.Report(true);
            ++counter;
         }
      }
      catch ( TaskCanceledException ) { }
      logger?.LogTrace("< PollAsync - final counter: {counter}", counter);
      return state;
   }


   /// <summary>
   /// TODO
   /// </summary>
   public static async Task PollAsync(this Func<CancellationToken, Task> pollFuncAsync,
                                      TimeSpan interval, CancellationToken cancellationToken, IProgress<bool>? progress = null, ILogger? logger = null) {
      logger?.LogTrace("> PollAsync (interval:{interval})", interval);
      int counter = 0;
      try {
         while ( !cancellationToken.IsCancellationRequested ) {
            logger?.LogTrace("> PollAsync [{counter}] action", counter);
            await pollFuncAsync(cancellationToken);
            logger?.LogTrace("< PollAsync [{counter}] action", counter);

            if ( cancellationToken.IsCancellationRequested ) break;

            logger?.LogTrace("> PollAsync [{counter}] delay", counter);
            await Task.Delay(interval, cancellationToken);
            logger?.LogTrace("< PollAsync [{counter}] delay", counter);

            progress?.Report(true);
            ++counter;
         }
      }
      catch ( TaskCanceledException ) { }
      logger?.LogTrace("< PollAsync - final counter: {counter}", counter);
   }
}
