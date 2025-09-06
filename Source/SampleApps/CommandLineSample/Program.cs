using System;
using System.CommandLine;
using Microsoft.Extensions.Logging;
using WelterKit.Extensions.SystemCommandLine;



namespace CommandLineSample {
   internal class Program {
      static void Main(string[] args)
         => new RootCommand("Get file system information")
           // .WithGlobalOption(new Option<LogLevel>("-v").WithAlias("--verbosity"),
           //                   out Option<LogLevel> verbosityOption)
          .WithSubcommand(new Command("summarize")
                               .WithAction(summarizeAction)
                          )
           // .WithOption(new Option<string>("--directory","--dir", "-d")
           //                                        .WithRequired(true),
           //             out Option<string> userIdOption)
           // .WithOption(new Option<string>("--of").WithAlias("--of-path")
           //                                       .WithRequired(true),
           //             out Option<string> pathOption)
           // .WithHandler(handleGetEffectivePerms, verbosityOption, userIdOption, pathOption)
           .Invoke(args);


      private static void summarizeAction(ParseResult obj) {
         throw new NotImplementedException();
      }
   }
}
