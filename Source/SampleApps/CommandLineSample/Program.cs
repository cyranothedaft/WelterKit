using System;
using System.CommandLine;
using System.IO;
using System.Text;
using WelterKit.Extensions.SystemCommandLine;
using WelterKit.Std;
using WelterKit.Std.Functional;



namespace CommandLineSample;

internal static class Program {
   static void Main(string[] args)
      => new RootCommand("Get file system information")
        .WithSubcommand(new Command("summarize")
                       .WithArgument(new Argument<DirectoryInfo>("directory") { DefaultValueFactory = _ => new DirectoryInfo(".") },
                                     out Argument<DirectoryInfo> directoryArgument)
                       .WithOption(new Option<bool>("--recurse","-r"),
                                   out  Option<bool> recurseOption)
                       .WithAction(parse => summarize(parse.GetValue(directoryArgument)!,
                                                      parse.GetValue(recurseOption)))
                       )
        .Parse(args)
        .Invoke();


   private static int summarize(DirectoryInfo directory, bool recurse)
      => run(() => FileSystem.Summarize(directory));


   private static int run(Func<Either<IError, StringBuilder>> command) {
      return command()
            .Map(handleResult)
            .Reduce(handleError);

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
