using System;
using System.CommandLine;
using System.IO;
using System.Text;
using WelterKit.Extensions.SystemCommandLine;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.Std;

namespace CommandLineSample;

internal static class Program {
   private static void Main(string[] args)
      => getCommandLineParser()
        .Parse(args)
        .Invoke();


   private static RootCommand getCommandLineParser()
      => new RootCommand("Get file system information")
           .WithSubcommand(new Command("summarize")
                          .WithArgument(new Argument<DirectoryInfo>("directory") { DefaultValueFactory = _ => new DirectoryInfo(".") },
                                        out Argument<DirectoryInfo> directoryArgument)
                          .WithOption(new Option<bool>("--recurse", "-r"),
                                      out Option<bool> recurseOption)
                          .WithAction(parse => summarize(parse.GetValue(directoryArgument)!,
                                                         parse.GetValue(recurseOption)))
                          );


   private static int summarize(DirectoryInfo directory, bool recurse)
      => run(() => FileSystem.Summarize(directory));


   private static int run(Func<Either<IError, StringBuilder>> command) {
      return command().Match(handleResult,
                             handleError);

      int handleResult(StringBuilder result) {
         Console.WriteLine(result);
         return 0; // 0 means success
      }

      int handleError(IError error) {
         Console.Error.WriteLine();
         Console.Error.WriteLine(error.DisplayText);
         return -1;
      }
   }
}
