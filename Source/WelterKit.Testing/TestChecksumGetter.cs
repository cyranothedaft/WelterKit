using System;
using WelterKit.Std;



namespace WelterKit.Testing {
   public class TestChecksumGetter : IChecksumGetter {
      public const string ChecksumStr = "n/a";

      public Checksum Get(FileSysFileItem fileItem) => Checksum.CreateForTest(ChecksumStr);
   }
}
