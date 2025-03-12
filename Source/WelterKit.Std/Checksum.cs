using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WelterKit.Std.ClassTools;
using WelterKit.Std.Diagnostics;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std {
   public enum ChecksumType {
      TestString,
      Sha256,
   }



   public interface IChecksumGetter {
      Checksum Get(FileSysFileItem fileItem);
   }


   // TODO: ? does this go in this file?
   public class ChecksumComputer : IChecksumGetter {
      public Checksum Get(FileSysFileItem fileItem) =>
            Checksum.ComputeChecksumForFile(fileItem);
   }


   public class Checksum : TooledClassBase {
      private const ChecksumType DefaultType = ChecksumType.Sha256;


      public ChecksumType Type { get; }
      public string ByteStr { get; }
      public byte[] Bytes { get; }


      private Checksum(ChecksumType type, string byteStr, byte[] bytes) {
         if ( type != ChecksumType.Sha256 &&
              type != ChecksumType.TestString )
            throw new ArgumentOutOfRangeException(nameof( type ), $"Unsupported ChecksumType: {type}");

         Type = type;
         ByteStr = byteStr.ToLowerInvariant();
         Bytes = bytes;
      }


      /// <summary>
      /// parameter in hexadecimal format: "a1b2c3"
      /// </summary>
      public Checksum(string byteStr, ChecksumType type = DefaultType) : this(type, byteStr, ByteUtil.HexToByteArray(byteStr)) { }

      public Checksum(byte[] bytes, ChecksumType type = DefaultType) : this(type, ByteUtil.ByteArrayToHex(bytes), bytes) { }


      /// <summary>
      /// Copy constructor
      /// </summary>
      private Checksum(Checksum obj) {
         Type = obj.Type;
         ByteStr = ( string )obj.ByteStr.Clone();
         Bytes = ( byte[] )obj.Bytes.Clone();
      }


      public static Checksum CreateForTest(string testString) =>
            new Checksum(ChecksumType.TestString, testString, Encoding.ASCII.GetBytes(testString));


      public static Checksum ComputeChecksumForFile(FileSysFileItem fileItem, ChecksumType type = DefaultType) =>
            ComputeChecksumForFile(fileItem.FilePath.ToString(), type);


      public static Checksum ComputeChecksumForFile(string filePath, ChecksumType type = DefaultType) {
         byte[] checksumBytes;
         using ( Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read) ) {
            checksumBytes = getHasher(type).ComputeHash(stream);
            stream.Close();
         }
         Logger.L.Write($"  - Computed checksum for file: {ByteUtil.ByteArrayToHex(checksumBytes)} : {DiagStr.StringInfo(filePath)}");
         return new Checksum(checksumBytes, type);
      }


      private static HashAlgorithm getHasher(ChecksumType type) {
         switch ( type ) {
            case ChecksumType.Sha256: return SHA256.Create();
            default:
               throw new ArgumentOutOfRangeException(nameof( type ), $"Unsupported ChecksumType: {type}");
         }
      }


      public Checksum Clone() => new Checksum(this);


      // === TODO?
      //public static bool Equals(Checksum x, Checksum y) {
      //   // consider nulls to be unequal
      //   if ( x == null && y == null ) return false;
      //   return Compare(x, y) == 0;
      //}


      #region ClassMeta

      public class Meta_Checksum : ClassMetaBase, ISingleton<Meta_Checksum>, IHasComparer<Checksum>, IHasDiagInfoGetter<Checksum> {
         public Meta_Checksum Get => new Meta_Checksum();
         static Meta_Checksum() { }
         private Meta_Checksum() { }

         GeneralComparatorBase<Checksum> IHasComparer<Checksum>.GetComparer() => Comparer;
         IDiagInfoGetter<Checksum> IHasDiagInfoGetter<Checksum>.GetDiagger() => DiagInfoGetter;
      }

      #endregion ClassMeta


      #region ClassTools

      private static GeneralComparatorBase<Checksum>? _comparer = null;
      public static GeneralComparatorBase<Checksum> Comparer => _comparer ??= new Comparator();

      private class Comparator : GeneralComparatorBase<Checksum> {
         protected override bool IsNullable => true;

         protected override IEnumerable<Func<Checksum, Checksum, int>> GetOrderedCompareOps() {
            yield return (x, y) => CompareUtil.CompareEnum(x.Type, y.Type);
            yield return (x, y) => CompareUtil.CompareSequence(x.Bytes, y.Bytes, CompareUtil.Compare);
         }
      }
      #endregion ClassTools


      #region Diagnostics

      private static IDiagInfoGetter<Checksum> _diagger = null;
      public static IDiagInfoGetter<Checksum> DiagInfoGetter => _diagger ??= new Diagger();

      private class Diagger : IDiagInfoGetter<Checksum> {
         public DebugInfoBase<Checksum> Info(Checksum obj) => Checksum.GetDebugInfo(obj);
      }


      public static DebugInfoBase<Checksum> GetDebugInfo(Checksum obj) => new DebugInfo(obj);
      private class DebugInfo : ComplexDebugInfoBase<Checksum> {
         public DebugInfo(Checksum obj)
               : base(obj, o => new (string, DebugInfoBase)[]
                                      {
                                            ( "Type", Diag.ObjectInfo(o.Type) ),
                                            ( "ByteStr", new StringInfo(o.ByteStr) ),
                                            ( "Bytes", Diag.ObjectListLine(o.Bytes) ),
                                      }) {
         }
      }

      public static DebugInfoBase<Checksum> GetDebugInfoShort(Checksum obj) => new DebugInfoLine(obj);
      private class DebugInfoLine : SimpleDebugInfoBase<Checksum> {
         public DebugInfoLine(Checksum obj) : base(obj) { }
         protected override string GetContents() => $"{_obj.Type}:{_obj.ByteStr}";
      }
      #endregion Diagnostics
   }
}



namespace WelterKit.Std.Diagnostics {
   public static partial class Diag {
      public static DebugInfoBase<Checksum> Info(Checksum obj) => Checksum.GetDebugInfo(obj);
      public static DebugInfoBase<Checksum> InfoShort(Checksum obj) => Checksum.GetDebugInfoShort(obj);
   }

   public static partial class DiagStr {
      public static string InfoShort(Checksum obj) => Diag.InfoShort(obj).ToString();
   }
}
