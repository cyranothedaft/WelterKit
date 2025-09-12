using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WelterKit.Std;
using WelterKit.Std.Functional;



namespace CommandLineSample;

internal static class FileSystem {
   public static Either<IError, StringBuilder> Summarize(DirectoryInfo directory) {
      try {
         return summarize(directory);
      }
      catch (Exception exception) {
         return new ExceptionError(exception, "summarizing files");
      }
   }


   private static StringBuilder summarize(DirectoryInfo directory) {
      IEnumerable<FileInfo> files = directory.EnumerateFiles();
      (int count, long size_bytes) totals = files.Aggregate((count: 0, size_bytes: 0L),
                                                            (accumulated, current) => (accumulated.count + 1, accumulated.size_bytes + current.Length));
      return new StringBuilder()
            .AppendLine($"Summary for directory:  {directory.FullName}")
            .AppendLine($"Total size: {totals.size_bytes}")
            .AppendLine($"Count     : {totals.count}");
   }
}
