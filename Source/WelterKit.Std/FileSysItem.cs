using System;
using System.Collections.Generic;
using System.IO;
using WelterKit.Std.ClassTools;
using WelterKit.Std.Diagnostics;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std {
   public enum FileSysItemType {
      File,
      Directory,
   }


   public abstract class FileSysItem : TooledClassBase {
      public FileSysItemType Type { get; }
      public BasedFilePath FilePath { get; }
      public DateTime FileTimeStamp_utc { get; }

      public string TypeChar => ( Type == FileSysItemType.Directory ) ? "d" : "F";
      public string RelativePath => FilePath.Tail;
      public string Name => Path.GetFileName(FullName);
      public string FullName => FilePath.ToString();


      protected FileSysItem(FileSysItemType type, BasedFilePath filePath, DateTime fileTimeStamp_utc) {
         Type = type;
         FilePath = filePath;
         FileTimeStamp_utc = fileTimeStamp_utc;
      }


      protected FileSysItem(FileSysItemType type, FileSystemInfo fileSystemInfo, string baseDir)
            : this(type, BasedFilePath.FromFullAndBase(fileSystemInfo.FullName, baseDir),
                   fileSystemInfo.LastWriteTimeUtc) {
      }


      public static FileSysItem FromFileSysInfo(FileSystemInfo fileSystemInfo, string baseDir) {
         return ( fileSystemInfo is DirectoryInfo )
                      ? ( FileSysItem )new FileSysDirItem(( DirectoryInfo )fileSystemInfo, baseDir)
                      : ( FileSysItem )new FileSysFileItem(( FileInfo )fileSystemInfo, baseDir);
      }


      #region ClassMeta

      public class Meta_FileSysItem : ClassMetaBase, ISingleton<Meta_FileSysItem>, IHasComparer<FileSysItem>, IHasDiagInfoGetter<FileSysItem> {
         public Meta_FileSysItem Get => new Meta_FileSysItem();
         static Meta_FileSysItem() { }
         private Meta_FileSysItem() { }

         GeneralComparatorBase<FileSysItem> IHasComparer<FileSysItem>.GetComparer() => Comparer;
         IDiagInfoGetter<FileSysItem> IHasDiagInfoGetter<FileSysItem>.GetDiagger() => DiagInfoGetter;
      }

      #endregion ClassMeta


      #region ClassTools

      private static GeneralComparatorBase<FileSysItem> _comparer = null,
                                                        _comparerFilePathOnly = null;
      public static GeneralComparatorBase<FileSysItem> Comparer => _comparer ?? ( _comparer = new Comparator() );
      public static GeneralComparatorBase<FileSysItem> ComparerFilePathOnly => _comparerFilePathOnly ?? ( _comparerFilePathOnly = new ComparatorFilePathOnly() );

      protected override IEnumerable<int> GetHashInts() {
         foreach ( int baseHashInt in base.GetHashInts() ) yield return baseHashInt;
         yield return ( int )this.Type;
         yield return this.FilePath.GetHashCode();
         yield return this.FileTimeStamp_utc.GetHashCode();
      }

      private class Comparator : GeneralComparatorBase<FileSysItem> {
         protected override bool IsNullable => true;

         protected override IEnumerable<Func<FileSysItem, FileSysItem, int>> GetOrderedCompareOps() {
            yield return (x, y) => CompareUtil.CompareEnum(x.Type, y.Type);
            yield return (x, y) => BasedFilePath.Comparer.Compare(x.FilePath, y.FilePath);
            yield return (x, y) => CompareUtil.Compare (x.FileTimeStamp_utc, y.FileTimeStamp_utc);
         }
      }

      // TODO: test
      private class ComparatorFilePathOnly : GeneralComparatorBase<FileSysItem> {
         protected override bool IsNullable => true;

         protected override IEnumerable<Func<FileSysItem, FileSysItem, int>> GetOrderedCompareOps() {
            yield return (x, y) => BasedFilePath.Comparer.Compare(x.FilePath, y.FilePath);
         }
      }
      #endregion ClassTools


      #region Diagnostics

      private static IDiagInfoGetter<FileSysItem> _diagger = null;
      public static IDiagInfoGetter<FileSysItem> DiagInfoGetter => _diagger ??= new Diagger();

      private class Diagger : IDiagInfoGetter<FileSysItem> {
         public DebugInfoBase<FileSysItem> Info(FileSysItem obj) => FileSysItem.GetDebugInfo(obj);
      }


      public static DebugInfoBase<FileSysItem> GetDebugInfo(FileSysItem obj)
         => ( obj is FileSysFileItem )
                  ? FileSysFileItem.GetDebugInfo(( FileSysFileItem )obj)
                  : FileSysDirItem.GetDebugInfo(( FileSysDirItem )obj);

      public static DebugInfoBase<FileSysItem> GetDebugInfoLine(FileSysItem obj)
         => ( obj is FileSysFileItem )
                  ? FileSysFileItem.GetDebugInfoLine(( FileSysFileItem )obj)
                  : FileSysDirItem.GetDebugInfoLine(( FileSysDirItem )obj);


      protected static string FormatLine(string typeChar, DateTime fileTimeStamp_utc, long? size, BasedFilePath filePath)
         => $"{typeChar}, " +
            $"{fileTimeStamp_utc.FormatForSync()}, " +
            $"{size?.ToString("0") ?? "",10}, " +
            $"{DiagStr.InfoShort(filePath)}";
      #endregion Diagnostics
   }



   public class FileSysFileItem : FileSysItem {
      public long Size { get; }


      public FileSysFileItem(FileInfo fileSystemInfo, string baseDir)
            : base(FileSysItemType.File, fileSystemInfo, baseDir) {
         Size = fileSystemInfo.Length;
      }


      public FileSysFileItem(BasedFilePath filePath, DateTime fileTimeStamp_utc, long size)
            : base(FileSysItemType.File, filePath, fileTimeStamp_utc) {
         Size = size;
      }


      public FileSysFileItem(string basedFilePathBase, string basedFilePathTail, DateTime fileTimeStamp_utc, long size)
            : this(new BasedFilePath(basedFilePathBase, basedFilePathTail), fileTimeStamp_utc, size) { }


      public static FileSysFileItem CreateForTest(string testBaseDir, string wardDir, string dir, string name, DateTime fileTimeStamp_utc, long fileSize) {
         string baseDir = Path.Combine(testBaseDir, wardDir),
                relativeFilePath = ( dir.Length > 0 ) ? Path.Combine(dir, name) : name;
         return new FileSysFileItem(new BasedFilePath(baseDir, relativeFilePath), fileTimeStamp_utc, fileSize);
      }


      public static FileSysFileItem CreateForTest(string baseDir, string relativeFilePath, DateTime fileTimeStamp_utc, long fileSize) {
         return new FileSysFileItem(new BasedFilePath(baseDir, relativeFilePath), fileTimeStamp_utc, fileSize);
      }


      #region ClassTools

      private static GeneralComparatorBase<FileSysFileItem> _comparer = null;
      public new static GeneralComparatorBase<FileSysFileItem> Comparer => _comparer ?? ( _comparer = new Comparator(FileSysItem.Comparer) );

      protected override IEnumerable<int> GetHashInts() {
         foreach ( int baseHashInt in base.GetHashInts() ) yield return baseHashInt;
         yield return this.Size.GetHashCode();
      }

      private class Comparator : GeneralComparatorBase<FileSysFileItem> {
         private readonly GeneralComparatorBase<FileSysItem> _baseComparer;
         public Comparator(GeneralComparatorBase<FileSysItem> baseComparer) {
            _baseComparer = baseComparer;
         }

         protected override bool IsNullable => true;

         protected override IEnumerable<Func<FileSysFileItem, FileSysFileItem, int>> GetOrderedCompareOps() {
            foreach ( var baseCompareOp in _baseComparer.OrderedCompareOps ) yield return baseCompareOp;
            yield return (x, y) => CompareUtil.Compare(x.Size, y.Size);
         }
      }
      #endregion ClassTools


      #region Diagnostics
      public static DebugInfoBase<FileSysItem> GetDebugInfo(FileSysFileItem obj) => new DebugInfo(obj);
      private class DebugInfo : ComplexDebugInfoBase<FileSysItem> {
         public DebugInfo(FileSysFileItem obj)
               : base(obj, o => new (string, DebugInfoBase)[]
                                      {
                                            ( "Type", Diag.ObjectInfo(o.TypeChar) ),
                                            ( "FilePath", Diag.InfoShort(o.FilePath) ),
                                            ( "FileTimeStamp_utc", new DateTimeInfo(o.FileTimeStamp_utc) ),
                                            ( "Size", Diag.ObjectInfo(( ( FileSysFileItem )o ).Size) ),
                                      }) {
         }
      }


      public static DebugInfoBase<FileSysItem> GetDebugInfoLine(FileSysFileItem obj) => new DebugInfoLine(obj);
      private class DebugInfoLine : SimpleDebugInfoBase<FileSysItem> {
         public DebugInfoLine(FileSysFileItem obj) : base(obj) { }
         protected override string GetContents() => FormatLine(_obj.TypeChar, _obj.FileTimeStamp_utc, ( ( FileSysFileItem )_obj ).Size, _obj.FilePath);
      }
      #endregion Diagnostics
   }


   public class FileSysDirItem : FileSysItem {
      public FileSysDirItem(string dirAsBase)
         : this(new DirectoryInfo(dirAsBase), dirAsBase) {
      }


      public FileSysDirItem(DirectoryInfo fileSystemInfo, string baseDir)
            : base(FileSysItemType.Directory, fileSystemInfo, baseDir) {
      }


      public FileSysDirItem(BasedFilePath filePath, DateTime fileTimeStamp_utc)
            : base(FileSysItemType.Directory, filePath, fileTimeStamp_utc) {
      }


      public FileSysDirItem(string basedFilePathBase, string basedFilePathTail, DateTime fileTimeStamp_utc)
            : this(new BasedFilePath(basedFilePathBase, basedFilePathTail), fileTimeStamp_utc) { }


      public static FileSysDirItem CreateForTest(string testBaseDir, string wardDir, string dir, string name, DateTime dateTime) {
         string baseDir = Path.Combine(testBaseDir, wardDir),
                relativeFilePath = ( dir.Length > 0 ) ? Path.Combine(dir, name) : name;
         return new FileSysDirItem(new BasedFilePath(baseDir, relativeFilePath), dateTime);
      }


      public static FileSysDirItem CreateForTest(string baseDir, string relativeFilePath, DateTime dateTime) {
         return new FileSysDirItem(new BasedFilePath(baseDir, relativeFilePath), dateTime);
      }


      #region Diagnostics
      public static DebugInfoBase<FileSysItem> GetDebugInfo(FileSysDirItem obj) => new DebugInfo(obj);
      private class DebugInfo : ComplexDebugInfoBase<FileSysItem> {
         public DebugInfo(FileSysDirItem obj)
               : base(obj, o => new (string, DebugInfoBase)[]
                                      {
                                            ( "Type", Diag.ObjectInfo(o.TypeChar) ),
                                            ( "FilePath", Diag.InfoShort(o.FilePath) ),
                                            ( "FileTimeStamp_utc", new DateTimeInfo(o.FileTimeStamp_utc) ),
                                      }) {
         }
      }


      public static DebugInfoBase<FileSysItem> GetDebugInfoLine(FileSysDirItem obj) => new DebugInfoLine(obj);
      private class DebugInfoLine : SimpleDebugInfoBase<FileSysItem> {
         public DebugInfoLine(FileSysDirItem obj) : base(obj) { }
         protected override string GetContents() => FormatLine(_obj.TypeChar, _obj.FileTimeStamp_utc, null, _obj.FilePath);
      }
      #endregion Diagnostics
   }
}



namespace WelterKit.Std.Diagnostics {
   partial class Diag {
      public static DebugInfoBase<FileSysItem> Info    (FileSysItem obj) => FileSysItem.GetDebugInfo    (obj);
      public static DebugInfoBase<FileSysItem> InfoLine(FileSysItem obj) => FileSysItem.GetDebugInfoLine(obj);

      public static ListDebugInfo<FileSysItem> InfoLineList(IList<FileSysItem> list) => new ListDebugInfo<FileSysItem>(list, Diag.InfoLine);
   }
   
   partial class DiagStr {
      public static string InfoLine(FileSysFileItem obj) => Diag.InfoLine(obj).ToString();
      public static string InfoLine(FileSysDirItem  obj) => Diag.InfoLine(obj).ToString();
   }
}
