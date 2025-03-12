using System;
using WelterKit.Functional;



namespace WelterKit {
   public class AppInfo {
      public Maybe<string> Title { get; }
      public Maybe<string> Version { get; }


      public AppInfo(string? title = null, string? version = null) {
         Title   = title   ?? AppInfoHelper.GetAppTitle();
         Version = version ?? AppInfoHelper.GetAppVersion();
      }
   }
}
