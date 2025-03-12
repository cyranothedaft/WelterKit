using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace WelterKit.BitConverters {
   [StructLayout(LayoutKind.Explicit)]
   [DebuggerDisplay("{ToString()}")]
   public readonly struct FourByteValue {

      [FieldOffset(0)]
      private readonly int _intValue;

      [FieldOffset(0)]
      private readonly uint _uintValue;


      public FourByteValue(int input) {
         _uintValue = 0;
         _intValue = input;
      }


      public FourByteValue(uint input) {
         _intValue = 0;
         _uintValue = input;
      }


      public override string ToString() => $"0x{_uintValue:x8}";


      public static uint ToUInt(int  intValue ) => ( FourByteValue )intValue;
      public static int  ToInt (uint uintValue) => ( FourByteValue )uintValue;


      public static implicit operator FourByteValue(int  intValue ) => new FourByteValue(intValue);
      public static implicit operator FourByteValue(uint uintValue) => new FourByteValue(uintValue);

      public static implicit operator int (FourByteValue fourByteValue) => fourByteValue._intValue;
      public static implicit operator uint(FourByteValue fourByteValue) => fourByteValue._uintValue;
   }
}
