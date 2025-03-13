using System;
using WelterKit.Std;
using WelterKit.Std.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Testing {
   public abstract class TesterBase {
      protected virtual string UnresolvedTempDir => @"%tmp%\sync4me_test";
      protected string TempDir => Environment.ExpandEnvironmentVariables(UnresolvedTempDir);

      [TestInitialize]
      public void Initialize() {
         Logger.Init_TextWriter(new AppInfo("test-util", "?"), Console.Out);
      }
   }
}
