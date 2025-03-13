using System;
using System.Threading.Tasks;


namespace WelterKit.Std.StaticUtilities {
   public static class DelegateUtil {
      public static TResult TryInvoke<TResult>(this Func<TResult> func, Func<Exception, TResult> handleException) {
         try                         { return func(); }
         catch (Exception exception) { return handleException(exception); }
      }


      public static async Task<TResult> TryInvoke<TResult>(this Func<Task<TResult>> asyncFunc, Func<Exception, TResult> handleException) {
         try                         { return await asyncFunc(); }
         catch (Exception exception) { return handleException(exception); }
      }


      public static TResult Wrap<TResult>(this Func<TResult> funcToWrap,
                                          Action? beforeAction = null,
                                          Action<TResult>? afterAction = null) {
         beforeAction?.Invoke();
         TResult result = funcToWrap();
         afterAction?.Invoke(result);
         return result;
      }


      public static TResult Wrap<TResult>(this Func<TResult> funcToWrap,
                                          Action? beforeAction = null,
                                          Action? afterAction = null) {
         beforeAction?.Invoke();
         TResult result = funcToWrap();
         afterAction?.Invoke();
         return result;
      }


      public static async Task<TResult> WrapAsync<TResult>(this Func<Task<TResult>> asyncFuncToWrap,
                                                           Action? beforeAction = null,
                                                           Action<TResult>? afterAction = null) {
         beforeAction?.Invoke();
         TResult result = await asyncFuncToWrap();
         afterAction?.Invoke(result);
         return result;
      }


      public static async Task<TResult> WrapAsync<TResult>(this Func<Task<TResult>> asyncFuncToWrap,
                                                           Action? beforeAction = null,
                                                           Action? afterAction = null) {
         beforeAction?.Invoke();
         TResult result = await asyncFuncToWrap();
         afterAction?.Invoke();
         return result;
      }


      public static void Wrap(this Action actionToWrap,
                              Action? beforeAction = null,
                              Action? afterAction = null)
         => Wrap(() => {
                    actionToWrap();
                    return 0;
                 },
                 beforeAction,
                 _ => { afterAction?.Invoke(); });


      public static async Task WrapAsync(this Func<Task> asyncActionToWrap,
                                         Action? beforeAction = null,
                                         Action? afterAction = null)
         => await WrapAsync(async () => {
                               await asyncActionToWrap();
                               return 0;
                            },
                            beforeAction,
                            _ => { afterAction?.Invoke(); });
   }
}
