using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
public class SampleUsage_ParseBinary1 {
   
   [TestMethod]
   public void ReadBinaryDataSample1() {
      byte[] binaryData = [ 0xff, 0xff, 0x2a, 0xff, 0xff ];
      ByteState initialState = new(binaryData, 0);
      (ByteState state, BinaryData readData) result = BinaryParser.ReadBinaryData().runState(initialState);
      Assert.AreEqual(new BinaryData(65535, 42, -1), result.readData);
   }
   
   
   
   internal record BinaryData(ushort Magic, byte Value1, short Value2);
   
   internal record ByteState(byte[] Buffer, int Offset);
   
   internal static class BinaryParser {
      public static State<ByteState, byte> ReadByte()
         => new (state => (state with { Offset = state.Offset + 1 }, state.Buffer[state.Offset]));
   
   
      public static State<ByteState, ushort> ReadUInt16()
         => (from b1 in ReadByte()
             from b2 in ReadByte()
             select (ushort)(b1 | (b2 << 8))).As();
   
   
      public static State<ByteState, short> ReadInt16()
         => (from b1 in ReadByte()
             from b2 in ReadByte()
             select (short)(b1 | (b2 << 8))).As();
   
   
      public static State<ByteState, BinaryData> ReadBinaryData()
         => (from magic in ReadUInt16()
             from value1 in ReadByte()
             from value2 in ReadInt16()
             select new BinaryData(magic, value1, value2)).As();
   }
}
