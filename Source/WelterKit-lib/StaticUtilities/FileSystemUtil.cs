using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;



namespace WelterKit.StaticUtilities {
   public static class FileSystemUtil {
      public static char[] InvalidFilePathChars { get; } = getInvalidFilePathChars();


      #region Enumeration
      // TODO: test
      public static IEnumerable<string> EnumFilesByExtension(string dir, string fileExtensionWithoutDot) {
         return Directory.Exists(dir)
                   ? Directory.EnumerateFiles(dir, "*." + fileExtensionWithoutDot)
                   : Enumerable.Empty<string>();
      }


      public static IEnumerable<FileSysItem> EnumFileSysItemsRecursively(string dir, bool includeRoot = false) {
         IEnumerable<FileSysItem> withRoot = includeRoot
                                                ? new List<FileSysItem>() { new FileSysDirItem(dir) }
                                                : new List<FileSysItem>() { };
         return withRoot.Concat(( new DirectoryInfo(dir) ).EnumerateFileSystemInfos("*.*", SearchOption.AllDirectories)
                                                          .Select(i => FileSysItem.FromFileSysInfo(i, dir)));
      }


      // TODO: test
      public static IEnumerable<FileSysFileItem> EnumFilesRecursively(string dir) {
         return ( new DirectoryInfo(dir) ).EnumerateFiles("*.*", SearchOption.AllDirectories)
                                          .Select(i => FileSysItem.FromFileSysInfo(i, dir))
                                          .Cast<FileSysFileItem>();
      }


      // TODO: test
      public static IEnumerable<FileSysDirItem> EnumDirsRecursively(string dir) {
         return ( new DirectoryInfo(dir) ).EnumerateDirectories("*.*", SearchOption.AllDirectories)
                                          .Select(i => FileSysItem.FromFileSysInfo(i, dir))
                                          .Cast<FileSysDirItem>();
      }


      // TODO: test
      public static IEnumerable<BasedFilePath> EnumBasedFilesAndDirsRecursively(string baseDir) {
         return Directory.EnumerateFileSystemEntries(baseDir, "*.*", SearchOption.AllDirectories)
                         .Select(f => BasedFilePath.FromFullAndBase(f, baseDir));
      }

      #endregion Enumeration


      public static void PurgeDir(string dir, Action<string> logAction = null) {
         const int RetryCount = 10;
         const int Delay_ms = 105;
         if ( Directory.Exists(dir) ) {
            TryPurgeDirResult lastResult = TryPurgeDirResult.None;

            // HACK: try a specified number of times, with a delay
            for ( int i = 0; i < RetryCount; ++i ) {
               logAction?.Invoke($"# Attempting to purge dir \"{dir}\"");
               lastResult = tryPurgeDir(dir);
               logAction?.Invoke($"# Attempted to purge dir  \"{dir}\", result:{lastResult}");

               if ( lastResult == TryPurgeDirResult.PurgedAndConfirmed ||
                    lastResult == TryPurgeDirResult.DirNoLongerExists )
                  break;
               Thread.Sleep(Delay_ms);
            }

            if ( lastResult.IsFailure() )
               throw new InvalidOperationException($"PurgeDir failed after {RetryCount} attempts; most recent result was {lastResult}.");
         }
         // reaching this point indicates success
      }


      internal enum TryPurgeDirResult {
         None,
         DirNoLongerExists,
         PurgedAndConfirmed,
         NoErrorButDirRemains,
         DirectoryNotEmptyException,
      }

      // rethrows if any exception occurs other than directory-not-empty
      private static TryPurgeDirResult tryPurgeDir(string dir) {
         if ( !Directory.Exists(dir) )
            return TryPurgeDirResult.DirNoLongerExists;

         try {
            Directory.Delete(dir, recursive: true);
         }
         catch ( IOException ex ) {
            if ( ex.HResult != -2147024751 ) // directory not empty
               throw; // any other exception - rethrow it
            return TryPurgeDirResult.DirectoryNotEmptyException;
         }

         return Directory.Exists(dir)
                      ? TryPurgeDirResult.NoErrorButDirRemains
                      : TryPurgeDirResult.PurgedAndConfirmed;
      }


      public static string? GetPathRelativeTo(this FileSysItem fileSysItem, string? relToPath) =>
            GetPathRelativeTo(fileSysItem.FullName, relToPath);


      public static string? GetPathRelativeTo(string? fullPath, string? relToPath) {
         if ( fullPath == null || relToPath == null )
            return null;
         else
            return fullPath.StartsWith(relToPath, true, CultureInfo.InvariantCulture)
               ? fullPath.Substring(relToPath.Length).Trim(Path.DirectorySeparatorChar)
               : fullPath;
      }


      internal static bool IsFailure(this TryPurgeDirResult finalTryPurgeDirResult) =>
            finalTryPurgeDirResult == TryPurgeDirResult.DirectoryNotEmptyException ||
            finalTryPurgeDirResult == TryPurgeDirResult.NoErrorButDirRemains;


      public static bool IsValidFilePathName(this string? id) =>
            id != null &&
            id.IndexOfAny(InvalidFilePathChars) == -1;


      // === TODO: lazy?
      private static char[] getInvalidFilePathChars() => Path.GetInvalidPathChars()
                                                             .Concat(Path.GetInvalidFileNameChars())
                                                             .Distinct()
                                                             .ToArray();


      public static string ResolveFullPath(string filePath) =>
            new FileInfo(filePath).FullName;


      public static bool IsDirectory(this FileSystemInfo info) =>
            ( info is DirectoryInfo ) || info.Attributes.HasFlag(FileAttributes.Directory);


      public static bool IsDirectory(string filePath) =>
            Directory.Exists(filePath);


      public static int GetDirectoryDepth(string path) {
         path = path?.Trim(Path.DirectorySeparatorChar);
         return string.IsNullOrWhiteSpace(path)
                   ? 0
                   : 1 + path.Count(c => c == Path.DirectorySeparatorChar);
      }


      // TODO: test
      public static void CopyDirContents(string sourceDir, string targetDir) {
         var sourceFilePaths = EnumBasedFilesAndDirsRecursively(sourceDir);
         foreach ( BasedFilePath sourceFilePath in sourceFilePaths ) {
            BasedFilePath targetFilePath = new BasedFilePath(targetDir, sourceFilePath.Tail);
            if ( IsDirectory(sourceFilePath.ToString()) )
               Directory.CreateDirectory(targetFilePath.ToString());
            else
               File.Copy(sourceFilePath.ToString(), targetFilePath.ToString());
         }
      }


      // TODO: test
      public static void CopyFile(string srcFilePath, string dstFilePath, bool overwrite, bool createDirectory) {
         if ( srcFilePath == null ) throw new ArgumentNullException(nameof( srcFilePath ));
         if ( dstFilePath == null ) throw new ArgumentNullException(nameof( dstFilePath ));
         if ( !File.Exists(srcFilePath) ) throw new FileNotFoundException("Source file doesn't exist", srcFilePath);

         string dstDir = Path.GetDirectoryName(dstFilePath);
         if ( !Directory.Exists(dstDir) ) {
            if ( createDirectory )
               Directory.CreateDirectory(dstDir);
            else
               throw new DirectoryNotFoundException("Target directory doesn't exist: " + dstDir);
         }

         File.Copy(srcFilePath, dstFilePath, overwrite);
      }


      public static bool ContainsInvalidFilenameChars(string filename)
         => filename != null
            && filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
   }
}
