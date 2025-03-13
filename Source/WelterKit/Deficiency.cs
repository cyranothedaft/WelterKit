using System;
using WelterKit.Std.Functional;


namespace WelterKit;


public record struct Deficiency(Deficiency.ReasonType Reason, Maybe<string> Text_) {
   public enum ReasonType {
      NeverObtained,
      NowObtaining,
      ErrorObtaining,
   }

   public static readonly Deficiency NeverObtained                = new Deficiency(ReasonType.NeverObtained , None.Value);
   public static readonly Deficiency NowObtaining                 = new Deficiency(ReasonType.NowObtaining  , None.Value);
   public static          Deficiency ErrorObtaining(string text) => new Deficiency(ReasonType.ErrorObtaining, text);

   public override string ToString()
      => Reason switch
            {
               ReasonType.NeverObtained  => "Never loaded",
               ReasonType.NowObtaining   => "Now loading...",
               ReasonType.ErrorObtaining => "Error loading",
               _                         => throw new ArgumentOutOfRangeException()
            }
       + Text_.Map(static text => " - " + text)
              .Reduce(string.Empty);
}


public static class DeficiencyExtensions {
   public static bool IsDeficient<R>     (this Either<Deficiency, R> either, Deficiency.ReasonType reason) => either.IsLeft(out Deficiency d) && d.Reason == reason;
   public static bool IsDeficient<R>     (this Either<Deficiency, R> either) => either.IsLeft();
   public static bool IsNeverObtained<R> (this Either<Deficiency, R> either) => either.IsDeficient(Deficiency.ReasonType.NeverObtained);
   public static bool IsNowObtaining<R>  (this Either<Deficiency, R> either) => either.IsDeficient(Deficiency.ReasonType.NowObtaining);
   public static bool IsErrorObtaining<R>(this Either<Deficiency, R> either) => either.IsDeficient(Deficiency.ReasonType.ErrorObtaining);

   public static string Format(this Deficiency deficiency)
      => deficiency.Reason switch {
         Deficiency.ReasonType.NeverObtained  => "(unobtained)",
         Deficiency.ReasonType.NowObtaining   => "(refreshing)",
         Deficiency.ReasonType.ErrorObtaining => deficiency.Text_
                                                           .Map(text => $"Error obtaining: {text}")
                                                           .Reduce("Error obtaining"),
         _ => throw new ArgumentOutOfRangeException()
      };
}
