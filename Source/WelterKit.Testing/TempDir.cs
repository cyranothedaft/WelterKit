using System;
using System.Diagnostics;
using System.IO;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Testing {
   // TODO: consoildate with FileSysTestLocation
   [DebuggerDisplay("{Dir}")]
   public sealed class TempDir : IDisposable {
      public string Dir { get; }


      public TempDir(string prefix = "") {
         string tempRoot = Environment.ExpandEnvironmentVariables("%tmp%");
         Dir = getNewRandomDirIn(tempRoot, prefix);

         //FileSystemUtil.PurgeDir(dir, Logger.L.Write);
         Directory.CreateDirectory(Dir);
      }


      private static string getNewRandomDirIn(string parentDir, string prefix) {
         string dir;
         do {
            dir = Path.Combine(parentDir, prefix + getRandomDirName());
         } while ( Directory.Exists(dir) );
         return dir;
      }


      private static readonly Random _rnd = new Random();
      private static readonly byte[] _buffer = new byte[6];
      private static string getRandomDirName() {
         _rnd.NextBytes(_buffer);
         return BitConverter.ToString(_buffer);
      }


      public void Dispose() {
         FileSystemUtil.PurgeDir(Dir);
      }
   }
}
