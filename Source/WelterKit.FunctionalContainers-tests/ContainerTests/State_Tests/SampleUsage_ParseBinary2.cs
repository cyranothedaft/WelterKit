using System;
using System.IO;
using System.Text;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
// Stream + BinaryReader
public class SampleUsage_ParseBinary {

   [TestMethod]
   public void ReadBinaryDataSample1() {
      byte[] rawBinaryData =
         [
            (byte)'M', (byte)'a', (byte)'g', (byte)'i', (byte)'c',
            0x2a, 0, 0, 0,
            0xff, 0xff,
         ];

      (ReadState state, MyBinaryData readData) result;
      using ( MemoryStream stream = new MemoryStream(rawBinaryData) )
      using ( BinaryReader reader = new BinaryReader(stream, new ASCIIEncoding()) ) {
         ReadState initialState = new(reader, 0);
         result = readMyBinaryData().runState(initialState);
      }
      Assert.AreEqual(new MyBinaryData("Magic", 42, -1), result.readData);
   }


      private static State<ReadState, MyBinaryData> readMyBinaryData()
         => (from magic  in Read<string>(string_ASCII(5))
             from value1 in Read(_uInt16)
             from value2 in Read(int16)
             select new MyBinaryData(magic, value1, value2)).As();


   internal record MyBinaryData(string Magic, int Value1, short Value2);

   private record ReadState(BinaryReader BinaryReader, uint Position);


   private static State<ReadState, byte> ReadByte()
      => new (state => (state with { Offset = state.Offset + 1 }, state.Buffer[state.Offset]));


   // private static State<ReadState, ushort> ReadUInt16() => Read(readUInt16);

   // private static (Func<BinaryReader, ushort> readValue, uint size) uInt16      ()            => (rdr => rdr.ReadUInt16()                                    , 2);
   private static Func<(Func<BinaryReader, ushort> readValue, uint size)> _uInt16 = () => (rdr => rdr.ReadUInt16()                                    , 2);

   // private static (Func<BinaryReader, short > readValue, uint size) int16       ()            => (rdr => rdr.ReadInt16()                                     , 2);
   private static Func<(Func<BinaryReader, string> value, uint size)> string_ASCII(uint length) => (rdr => Encoding.ASCII.GetString(rdr.ReadBytes((int)length)), length);
   // private static (Func<uint,BinaryReader, string> readValue, uint size) string_ASCII()=> ((len, rdr) => Encoding.ASCII.GetString(rdr.ReadBytes((int)len)), len);


   private static State<ReadState, T> Read<T>(Func<BinaryReader, T> getValue, uint size)
      => new(state => {
                // (Func<BinaryReader, T> getvalue, uint size) = func();
                return (state with { Position = state.Position + getValue.size },
                        value: getValue.func(state.BinaryReader));
             });



   // private static State<ReadState, T> Read<T>(Func<(Func<BinaryReader, T> value, uint size)> func)
   //    => new(state => {
   //              (Func<BinaryReader, T> getvalue, uint size) = func();
   //              return (state with { Position = state.Position + size },
   //                      value: getvalue(state.BinaryReader));
   //           });


   // private static State<ReadState, ushort> Read(Func<(Func<BinaryReader, ushort> value, uint size)> func) {
   //    State<ReadState, ushort> r = new State<ReadState, ushort>(state => {
   //       (Func<BinaryReader, ushort> getvalue, uint size)  = func();
   //                                                        return (state with { Position = state.Position + size },
   //                                                                         value: getvalue(state.BinaryReader));
   //                                                              });
   //    return r;
   // }
   //
   //
   private static State<ReadState, short> ReadInt16()
      => (from b1 in ReadByte()
          from b2 in ReadByte()
          select (short)(b1 | (b2 << 8))).As();
}
