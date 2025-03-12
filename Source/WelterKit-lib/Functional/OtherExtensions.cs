using System;
using System.Threading.Tasks;


namespace WelterKit.Functional {
   public static class OtherExtensions {
      public static TResult MapTo<T, TResult>(this T obj, Func<T, TResult> mapFunc)
         => mapFunc(obj);


      public static async Task<TResult> MapToAsync<T, TResult>(this T obj, Func<T, Task<TResult>> mapAsyncFunc)
         => await mapAsyncFunc(obj);


      public static T MapThrough<T>(this T obj, Action<T> action) {
         action(obj);
         return obj;
      }


      public static TBuilder When<TBuilder>(this TBuilder builder, Func<bool> predicate, Func<TBuilder, TBuilder> builderAction)
         => predicate()
                  ? builderAction(builder)
                  : builder;
   }
}
