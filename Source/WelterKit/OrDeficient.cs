using System;
using WelterKit.Std.Functional;

namespace WelterKit;


public record struct OrDeficient<T>(Either<Deficiency, T> Either) {
   //===
}


public static class OrDeficientExtensions {
   public static OrDeficient<T> OrDeficient<T>(this T obj) => new OrDeficient<T>(obj);
}
