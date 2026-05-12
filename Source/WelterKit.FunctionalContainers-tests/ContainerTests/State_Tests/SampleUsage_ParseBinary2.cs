using System;
using System.IO;
using System.Text;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework.Traits;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
// Stream + BinaryReader
public class SampleUsage_ParseBinary2 {

   [TestMethod]
   public void ReadBinaryDataSample() {
      byte[] rawBinaryData =
         [
            (byte)'M',
            (byte)'a',
            (byte)'g',
            (byte)'i',
            (byte)'c',
            0x2a, 0, 0, 0,
            0xff, 0xff,
         ];

      (BinaryReaderWithState.ReadState state, MyBinaryData readData) result;
      using ( MemoryStream stream = new MemoryStream(rawBinaryData) )
      using ( BinaryReader reader = new BinaryReader(stream, new ASCIIEncoding()) ) // TODO: also encapsulate (abstract away) the BinaryReader itself
         result = readMyBinaryData()
                 .runState(new BinaryReaderWithState.ReadState(reader, Position: 0));

      Assert.AreEqual(new MyBinaryData("Magic", 42, -1), result.readData);
      Assert.AreEqual((uint)rawBinaryData.Length,        result.state.Position);
   }


   private static State<BinaryReaderWithState.ReadState, MyBinaryData> readMyBinaryData()
      => (from magic  in BinaryReaderWithState.Read(Readers.FixedLengthString_ASCII, 5)
          from value1 in BinaryReaderWithState.Read(Readers.Int32)
          from value2 in BinaryReaderWithState.Read(Readers.Int16)
          select new MyBinaryData(magic, value1, value2)
         ).As();


   internal record MyBinaryData(string Magic, int Value1, short Value2);



   internal static class BinaryReaderWithState {

      internal record ReadState(BinaryReader BinaryReader, uint Position);


      public static State<ReadState, T> Read<T>(Func<(Func<BinaryReader, T> getValue, uint size)> func)
         => new(state => {
                   (Func<BinaryReader, T> getvalue, uint size) = func();
                   return (state with { Position = state.Position + size },
                           value: getvalue(state.BinaryReader));
                });


      public static State<ReadState, T> Read<T>(Func<Func<uint, BinaryReader, T>> getValue, uint size)
         => new(state => {
                   Func<uint, BinaryReader, T> gv = getValue(); // TODO: ICK
                   return (state with { Position = state.Position + size },
                           value: gv(size, state.BinaryReader));
                });
   }


   internal static class Readers {
      public static (Func<BinaryReader, short  > readValue, uint size) Int16() => (rdr => rdr.ReadInt16  (), 2);
      public static (Func<BinaryReader, int    > readValue, uint size) Int32() => (rdr => rdr.ReadInt32  (), 4);

      public static Func<uint, BinaryReader, string> FixedLengthString_ASCII() => (len, rdr) => Encoding.ASCII.GetString(rdr.ReadBytes((int)len));
   }
}
