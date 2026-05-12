// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using WelterKit.FunctionalContainers_tests.ContainerTests.Writer_Tests;
// using WelterKit.FunctionalContainers.Containers;
// using WelterKit.FunctionalContainers.Framework;
// using WelterKit.FunctionalContainers.Framework.Traits;
//
//
//
// namespace WelterKit.FunctionalContainers_tests.ContainerTests.SampleUsage ;
//
// [TestClass]
// public class ReadDataWithStateAndLog {
//
//    private record AppMonad<A>=====
//
//    [TestMethod]
//    public void ReadBinaryDataSample1() {
//       byte[] rawBinaryData =
//          [
//             (byte)'M',
//             (byte)'a',
//             (byte)'g',
//             (byte)'i',
//             (byte)'c',
//             0x2a, 0, 0, 0,
//             0xff, 0xff,
//          ];
//
//
//
//
//
//       (MyBinaryData data, uint readerPosition) final = readData(rawBinaryData);
//
//       (MyBinaryData data, uint readerPosition) readData(byte[] bytes) {
//          (BinaryReaderWithState.ReadState state, MyBinaryData data) result;
//          using ( MemoryStream stream = new MemoryStream(rawBinaryData) )
//          using ( BinaryReader reader = new BinaryReader(stream, new ASCIIEncoding()) ) // TODO: also encapsulate (abstract away) the BinaryReader itself
//             result = readMyBinaryData()
//                  .runState(new BinaryReaderWithState.ReadState(reader, Position: 0));
//          return (result.data, result.state.Position);
//       }
//
//
//
//       (StringAccumulator writer, int value) finalResult = doSomeMultiplying(3, 5)
//                                                          .RunWriter();
//       Assert.AreEqual(3 * 5, finalResult.value);
//       StringAccumulatorAssert.AreEqual(new(["Logged: 3", "Logged: 5"]), finalResult.writer);
//       return;
//
//
//       // A simple function that logs its input
//       static Writer<StringAccumulator, int> logValue(int value)
//          => NewWriter([$"Logged: {value}"], value);
//
//       static Writer<StringAccumulator, int> doSomeMultiplying(int value1, int value2)
//          => (from a in logValue(value1)
//              from b in logValue(value2)
//              select a * b).As();
//
//
//       static State<BinaryReaderWithState.ReadState, MyBinaryData> readMyBinaryData()
//          => (from magic  in BinaryReaderWithState.Read(Readers.FixedLengthString_ASCII, 5)
//              from value1 in BinaryReaderWithState.Read(Readers.Int32)
//              from value2 in BinaryReaderWithState.Read(Readers.Int16)
//              select new MyBinaryData(magic, value1, value2)
//             ).As();
//
//
//       Assert.AreEqual(3*5, finalResult.value);
//       StringAccumulatorAssert.AreEqual(new(["Logged: 3", "Logged: 5"]), finalResult.writer);
//
//       Assert.AreEqual(new MyBinaryData("Magic", 42, -1), result.readData);
//       Assert.AreEqual((uint)rawBinaryData.Length,        result.state.Position);
//    }
//
//
//
//    internal record MyBinaryData(string Magic, int Value1, short Value2);
//
//
//
//    internal static class BinaryReaderWithState {
//
//       internal record ReadState(BinaryReader BinaryReader, uint Position);
//
//
//
//       public static State<ReadState, T> Read<T>(Func<(Func<BinaryReader, T> getValue, uint size)> func)
//          => new(state => {
//                    (Func<BinaryReader, T> getvalue, uint size) = func();
//                    return (state with
//                               {
//                                  Position = state.Position + size
//                               },
//                            value: getvalue(state.BinaryReader));
//                 });
//
//
//       public static State<ReadState, T> Read<T>(Func<Func<uint, BinaryReader, T>> getValue, uint size)
//          => new(state => {
//                    Func<uint, BinaryReader, T> gv = getValue(); // TODO: ICK
//                    return (state with
//                               {
//                                  Position = state.Position + size
//                               },
//                            value: gv(size, state.BinaryReader));
//                 });
//    }
//
//
//
//    internal static class Readers {
//       public static (Func<BinaryReader, short> readValue, uint size) Int16() => (rdr => rdr.ReadInt16(), 2);
//
//       public static (Func<BinaryReader, int> readValue, uint size) Int32() => (rdr => rdr.ReadInt32(), 4);
//
//       public static Func<uint, BinaryReader, string> FixedLengthString_ASCII() => (len, rdr) => Encoding.ASCII.GetString(rdr.ReadBytes((int)len));
//    }
// }
