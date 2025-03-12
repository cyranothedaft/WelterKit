using System;
using System.IO;
using WelterKit.Diagnostics;
using WelterKit.StaticUtilities;



namespace WelterKit.Testing {
   public class FileSysTestLocation : IDisposable {
      public string Dir { get; }


      /// <summary>
      /// Creates the given directory after purging any existing contents.
      /// </summary>
      public FileSysTestLocation(string dir) {
         Dir = dir;

         FileSystemUtil.PurgeDir(dir, Logger.L.Write);
         Directory.CreateDirectory(dir);
      }


      public void Dispose() {
         FileSystemUtil.PurgeDir(Dir);
      }
   }
}
