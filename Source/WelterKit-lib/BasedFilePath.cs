using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WelterKit.ClassTools;
using WelterKit.Diagnostics;
using WelterKit.StaticUtilities;



namespace WelterKit {
   public class BasedFilePath : TooledClassBase {
      public string Base { get; }
      public string Tail { get; }


      public BasedFilePath(string? @base, string? tail) {
         Base = @base ?? "";
         Tail = tail?.RemoveStart("\\")?.RemoveEnd("\\") ?? "";
      }


      public BasedFilePath((string, string ) parts)
            : this(parts.Item1, parts.Item2) {
      }


      public override string ToString() =>
            Path.Combine(Base, Tail);


      public string ToStringWithNewBase(string newBase) =>
            this.ReplaceBase(newBase).ToString();


      public static BasedFilePath FromBaseRoot(string @base) =>
            new BasedFilePath(@base, "");


      public static BasedFilePath? FromFullAndBase(string? fullPath, string? @base) {
         if ( fullPath == null || @base == null )
            return null;
         else
            return fullPath.StartsWith(@base, true, CultureInfo.InvariantCulture)
                         ? new BasedFilePath(@base, fullPath.Substring(@base.Length).Trim(Path.DirectorySeparatorChar))
                         : new BasedFilePath("", fullPath);
      }


      public static BasedFilePath FromFullAndTail(string fullPath, string tail) =>
            new BasedFilePath(fullPath.SplitForSuffix(tail));


      public BasedFilePath ReplaceBase(string newBase) =>
            new BasedFilePath(newBase, this.Tail);


      public BasedFilePath GetParentDirectory() =>
            new BasedFilePath(this.Base, this.Tail.Length > 0
                                               ? Path.GetDirectoryName(this.Tail)
                                               : "");


      public bool FileExists()
         => File.Exists(this.ToString());


      #region ClassMeta

      public class Meta_BasedFilePath : ClassMetaBase, ISingleton<Meta_BasedFilePath>, IHasComparer<BasedFilePath>, IHasDiagInfoGetter<BasedFilePath> {
         public Meta_BasedFilePath Get => new Meta_BasedFilePath();
         static Meta_BasedFilePath() { }
         private Meta_BasedFilePath() { }

         GeneralComparatorBase<BasedFilePath> IHasComparer<BasedFilePath>.GetComparer() => Comparer;
         IDiagInfoGetter<BasedFilePath> IHasDiagInfoGetter<BasedFilePath>.GetDiagger() => DiagInfoGetter;
      }

      #endregion ClassMeta


      #region ClassTools

      private static GeneralComparatorBase<BasedFilePath>? _comparer = null;
      public static GeneralComparatorBase<BasedFilePath> Comparer => _comparer ??= new Comparator();

      protected override IEnumerable<int> GetHashInts() {
         yield return this.Base.GetHashCode();
         yield return this.Tail.GetHashCode();
      }


      private class Comparator : GeneralComparatorBase<BasedFilePath> {
         protected override bool IsNullable => true;

         protected override IEnumerable<Func<BasedFilePath, BasedFilePath, int>> GetOrderedCompareOps() {
            yield return (x, y) => string.Compare(x.Base, y.Base, StringComparison.CurrentCultureIgnoreCase);
            yield return (x, y) => string.Compare(x.Tail, y.Tail, StringComparison.CurrentCultureIgnoreCase);
         }
      }
      #endregion ClassTools


      #region Diagnostics

      private static IDiagInfoGetter<BasedFilePath>? _diagger = null;
      public static IDiagInfoGetter<BasedFilePath> DiagInfoGetter => _diagger ??= new Diagger();

      private class Diagger : IDiagInfoGetter<BasedFilePath> {
         public DebugInfoBase<BasedFilePath> Info(BasedFilePath obj) => BasedFilePath.GetDebugInfo(obj);
      }


      public static DebugInfoBase<BasedFilePath> GetDebugInfo(BasedFilePath obj) => new DebugInfo(obj);
      private class DebugInfo : ComplexDebugInfoBase<BasedFilePath> {
         public DebugInfo(BasedFilePath obj)
               : base(obj, o => new (string, DebugInfoBase)[]
                                      {
                                            ( "Base", new Diagnostics.StringInfo(o.Base) ),
                                            ( "Tail", new Diagnostics.StringInfo(o.Tail) ),
                                      }) {
         }
      }


      public static DebugInfoBase<BasedFilePath> GetDebugInfoShort(BasedFilePath obj) => new DebugInfoShort(obj);
      private class DebugInfoShort : SimpleDebugInfoBase<BasedFilePath> {
         public DebugInfoShort(BasedFilePath obj) : base(obj) { }
         protected override string GetContents() => $"{DiagStr.StringInfo(_obj.Base)}+" +
                                                    $"{DiagStr.StringInfo(_obj.Tail)}";
      }

      public static ListDebugInfo<BasedFilePath> GetDebugInfoShortList(IList<BasedFilePath> list, string? alt = null)
         => new ListDebugInfo<BasedFilePath>(list, Diag.InfoShort, altEmptyText: alt);


      public static DebugInfoBase<BasedFilePath> GetDebugInfoShort2(BasedFilePath obj) => new DebugInfoShort2(obj);
      private class DebugInfoShort2 : SimpleDebugInfoBase<BasedFilePath> {
         public DebugInfoShort2(BasedFilePath obj) : base(obj) { }
         protected override string GetContents() => $"{DiagStr.StringInfo(_obj.Tail)} under " +
                                                    $"{DiagStr.StringInfo(_obj.Base)}";
      }

      #endregion Diagnostics
   }
}



namespace WelterKit.Diagnostics {
   partial class Diag {
      public static DebugInfoBase<BasedFilePath> InfoShort (BasedFilePath obj) => BasedFilePath.GetDebugInfoShort (obj);
      public static DebugInfoBase<BasedFilePath> InfoShort2(BasedFilePath obj) => BasedFilePath.GetDebugInfoShort2(obj);

      public static ListDebugInfo<BasedFilePath> InfoShortList(IList<BasedFilePath> list, string? alt = null)
         => BasedFilePath.GetDebugInfoShortList(list, alt);
   }
   
   partial class DiagStr {
      public static string InfoShort (BasedFilePath obj) => Diag.InfoShort (obj).ToString();
      public static string InfoShort2(BasedFilePath obj) => Diag.InfoShort2(obj).ToString();
   }
}
