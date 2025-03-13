using System;
using WelterKit.Std.Functional;



namespace WelterKit.Std {
   public class AppInfo {
      public Maybe<string> Title { get; }
      public Maybe<string> Version { get; }


      public AppInfo(string? title = null, string? version = null) {
         Title   = title   ?? AppInfoHelper.GetAppTitle();
         Version = version ?? AppInfoHelper.GetAppVersion();
      }
   }
}
