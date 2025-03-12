using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_CompareUtil {
      [TestMethod]
      public void CompareNulls_bothNull() {
         int c = 1234;
         Assert.IsTrue(CompareUtil.CompareNulls(null, null, ref c));
         Assert.AreEqual(0, c);
      }


      [TestMethod]
      public void CompareNulls_oneNull_1() {
         int c = 1234;
         Assert.IsTrue(CompareUtil.CompareNulls(null, "1", ref c));
         Assert.AreEqual(-1, c);
      }


      [TestMethod]
      public void CompareNulls_oneNull_2() {
         int c = 1234;
         Assert.IsTrue(CompareUtil.CompareNulls("1", null, ref c));
         Assert.AreEqual(1, c);
      }


      [TestMethod]
      public void CompareNulls_noNulls_1() {
         int c = 1234;
         Assert.IsFalse(CompareUtil.CompareNulls("1", "2", ref c));
         Assert.AreEqual(1234, c);
      }



      private enum TestEnum1 {
         val0 = 0,
         val1 = 1,
      }

      [TestMethod]
      public void CompareEnum_1() {
         Assert.AreEqual(0, CompareUtil.CompareEnum(TestEnum1.val0, TestEnum1.val0));
         Assert.AreEqual(-1, CompareUtil.CompareEnum(TestEnum1.val0, TestEnum1.val1));
         Assert.AreEqual(1, CompareUtil.CompareEnum(TestEnum1.val1, TestEnum1.val0));
      }


      [TestMethod]
      public void CompareBool() {
         Assert.AreEqual(0, CompareUtil.Compare(false, false));
         Assert.AreEqual(0, CompareUtil.Compare(true, true));
         Assert.IsTrue(0 > CompareUtil.Compare(false, true));
         Assert.IsTrue(0 < CompareUtil.Compare(true, false));
      }


      [TestMethod]
      public void CompareByte() {
         Assert.AreEqual(0, CompareUtil.Compare(( byte )0, ( byte )0));
         Assert.AreEqual(0, CompareUtil.Compare(( byte )1, ( byte )1));
         Assert.AreEqual(0, CompareUtil.Compare(( byte )0xff, ( byte )0xff));
         Assert.IsTrue(0 < CompareUtil.Compare(( byte )0xff, ( byte )0));
         Assert.IsTrue(0 < CompareUtil.Compare(( byte )0xff, ( byte )1));
         Assert.IsTrue(0 > CompareUtil.Compare(( byte )0, ( byte )0xff));
         Assert.IsTrue(0 > CompareUtil.Compare(( byte )0, ( byte )1));
         Assert.IsTrue(0 > CompareUtil.Compare(( byte )1, ( byte )0xff));
         Assert.IsTrue(0 < CompareUtil.Compare(( byte )1, ( byte )0));
      }


      [TestMethod]
      public void CompareInt() {
         Assert.AreEqual(0, CompareUtil.Compare(( int )0, ( int )0));
         Assert.AreEqual(0, CompareUtil.Compare(( int )1, ( int )1));
         Assert.AreEqual(0, CompareUtil.Compare(( int )-1, ( int )-1));
         Assert.IsTrue(0 > CompareUtil.Compare(( int )-1, ( int )0));
         Assert.IsTrue(0 > CompareUtil.Compare(( int )-1, ( int )1));
         Assert.IsTrue(0 < CompareUtil.Compare(( int )0, ( int )-1));
         Assert.IsTrue(0 > CompareUtil.Compare(( int )0, ( int )1));
         Assert.IsTrue(0 < CompareUtil.Compare(( int )1, ( int )-1));
         Assert.IsTrue(0 < CompareUtil.Compare(( int )1, ( int )0));

         Assert.AreEqual(0, CompareUtil.Compare(int.MinValue, int.MinValue));
         Assert.AreEqual(0, CompareUtil.Compare(int.MaxValue, int.MaxValue));
         Assert.IsTrue(0 < CompareUtil.Compare(( int )0, int.MinValue));
         Assert.IsTrue(0 > CompareUtil.Compare(( int )0, int.MaxValue));
         Assert.IsTrue(0 > CompareUtil.Compare(int.MinValue, ( int )0));
         Assert.IsTrue(0 > CompareUtil.Compare(int.MinValue, int.MaxValue));
         Assert.IsTrue(0 < CompareUtil.Compare(int.MaxValue, int.MinValue));
         Assert.IsTrue(0 < CompareUtil.Compare(int.MaxValue, ( int )0));
      }


      [TestMethod]
      public void CompareLong() {
         Assert.AreEqual(0, CompareUtil.Compare(long.MinValue, long.MinValue));
         Assert.AreEqual(0, CompareUtil.Compare(long.MaxValue, long.MaxValue));
         Assert.IsTrue(0 < CompareUtil.Compare(0L, long.MinValue));
         Assert.IsTrue(0 > CompareUtil.Compare(0L, long.MaxValue));
         Assert.IsTrue(0 > CompareUtil.Compare(long.MinValue, 0L));
         Assert.IsTrue(0 > CompareUtil.Compare(long.MinValue, long.MaxValue));
         Assert.IsTrue(0 < CompareUtil.Compare(long.MaxValue, long.MinValue));
         Assert.IsTrue(0 < CompareUtil.Compare(long.MaxValue, 0L));

         Assert.AreEqual(0, CompareUtil.Compare(0L, 0L));
         Assert.AreEqual(0, CompareUtil.Compare(1L, 1L));
         Assert.AreEqual(0, CompareUtil.Compare(-1L, -1L));
         Assert.IsTrue(0 > CompareUtil.Compare(-1L, 0L));
         Assert.IsTrue(0 > CompareUtil.Compare(-1L, 1L));
         Assert.IsTrue(0 < CompareUtil.Compare(0L, -1L));
         Assert.IsTrue(0 > CompareUtil.Compare(0L, 1L));
         Assert.IsTrue(0 < CompareUtil.Compare(1L, -1L));
         Assert.IsTrue(0 < CompareUtil.Compare(1L, 0L));
      }


      [TestMethod]
      public void CompareDateTime() {
         DateTime dt1 = new DateTime(1, 2, 3, 4, 5, 6),
                  dt2 = new DateTime(9, 8, 7, 6, 5, 4);

         Assert.AreEqual(0, CompareUtil.Compare(DateTime.MinValue, DateTime.MinValue));
         Assert.AreEqual(0, CompareUtil.Compare(DateTime.MaxValue, DateTime.MaxValue));
         Assert.AreEqual(0, CompareUtil.Compare(dt1, dt1));
         Assert.AreEqual(0, CompareUtil.Compare(dt2, dt2));

         Assert.IsTrue(CompareUtil.Compare(dt1, dt2) < 0);
         Assert.IsTrue(CompareUtil.Compare(DateTime.MinValue, dt1) < 0);
         Assert.IsTrue(CompareUtil.Compare(DateTime.MinValue, dt2) < 0);
         Assert.IsTrue(CompareUtil.Compare(dt1, DateTime.MaxValue) < 0);
         Assert.IsTrue(CompareUtil.Compare(dt2, DateTime.MaxValue) < 0);
      }


      [TestMethod]
      public void CompareType_nulls() {
         Assert.AreEqual(0, CompareUtil.Compare(( Type )null, ( Type )null));
         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( int )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( string )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( CompareUtil )) < 0);
      }


      [TestMethod]
      public void CompareType() {
         Assert.AreEqual(0, CompareUtil.Compare(typeof( int ), typeof( int )));
         Assert.AreEqual(0, CompareUtil.Compare(typeof( string ), typeof( string )));
         Assert.AreEqual(0, CompareUtil.Compare(typeof( CompareUtil ), typeof( CompareUtil )));

         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( int )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( string )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( Type )null, typeof( CompareUtil )) < 0);
      }


      [TestMethod]
      public void CompareTupleString_nulls() {
         Assert.AreEqual(0, CompareUtil.Compare(( ( string )null, ( string )null ), ( ( string )null, ( string )null )));
         Assert.AreEqual(0, CompareUtil.Compare(( ( string )null, ""             ), ( ( string )null, ""             )));
         Assert.AreEqual(0, CompareUtil.Compare(( ""            , ( string )null ), ( ""            , ( string )null )));

         Assert.IsTrue(0 > CompareUtil.Compare(( ( string )null, ( string )null ), ( ( string )null, "" )));
         Assert.IsTrue(0 > CompareUtil.Compare(( ( string )null, ( string )null ), ( ""            , ( string )null )));
         Assert.IsTrue(0 > CompareUtil.Compare(( ( string )null, ( string )null ), ( ""            , "" )));
      }


      [TestMethod]
      public void CompareTupleString() {
         Assert.AreEqual(0, CompareUtil.Compare(( "", "" ), ( "", "" )));
         Assert.AreEqual(0, CompareUtil.Compare(( "a", "a" ), ( "a", "a" )));
         Assert.AreEqual(0, CompareUtil.Compare(( "xyz", "xyz" ), ( "xyz", "xyz" )));

         Assert.IsTrue(CompareUtil.Compare(( "a", "a" ), ( "" , ""  )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "a", "a" ), ( "" , "a" )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "a", "a" ), ( "a", ""  )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "a", ""  ), ( "" , ""  )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "a", ""  ), ( "" , "a" )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "a", ""  ), ( "a", "a" )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( "" , "a" ), ( "" , ""  )) > 0);
         Assert.IsTrue(CompareUtil.Compare(( "" , "a" ), ( "a", ""  )) < 0);
         Assert.IsTrue(CompareUtil.Compare(( "" , "a" ), ( "a", "a" )) < 0);
      }


      [TestMethod]
      public void CompareSequenceInt() {
         Assert.AreEqual(0, CompareUtil.CompareSequence<int>(null, null, CompareUtil.Compare));
         Assert.AreEqual(0, CompareUtil.CompareSequence<int>(new int[] { }, new int[] { }, CompareUtil.Compare));
         Assert.AreEqual(0, CompareUtil.CompareSequence<int>(new int[] { 0 }, new int[] { 0 }, CompareUtil.Compare));
         Assert.AreEqual(0, CompareUtil.CompareSequence<int>(new int[] { 0, int.MinValue, int.MaxValue }, new int[] { 0, int.MinValue, int.MaxValue }, CompareUtil.Compare));

         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(null, new int[] { }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(null, new int[] { 0 }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(null, new int[] { 0, int.MinValue, int.MaxValue }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(new int[] { }, new int[] { 0 }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(new int[] { }, new int[] { 0, int.MinValue, int.MaxValue }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(new int[] { 0 }, new int[] { 1 }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(new int[] { 0 }, new int[] { 0, 1 }, CompareUtil.Compare));
         Assert.IsTrue(0 > CompareUtil.CompareSequence<int>(new int[] { 0 }, new int[] { 0, int.MinValue, int.MaxValue }, CompareUtil.Compare));
      }


      [TestMethod]
      public void CompareSetInt() {
         IEqualityComparer<int> intComparer = new CompareUtil.IntComparer();

         // equal as both sequences and sets
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(null,                     null,                     intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { },            new int[] { },            intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 0 },          new int[] { 0 },          intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 0, -99, 99 }, new int[] { 0, -99, 99 }, intComparer));

         // unequal as both sequences and sets
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(null,            new int[] { },            intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(null,            new int[] { 0 },          intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(null,            new int[] { 0, -99, 99 }, intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(new int[] { },   new int[] { 0 },          intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(new int[] { },   new int[] { 0, -99, 99 }, intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(new int[] { 0 }, new int[] { 1 },          intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(new int[] { 0 }, new int[] { 0, 1 },       intComparer));
         Assert.AreNotEqual(0, CompareUtil.CompareSet<int>(new int[] { 0 }, new int[] { 0, -99, 99},  intComparer));

         // unequal sequences but equal sets - different order
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 11, 33, 22 }, intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 22, 11, 33 }, intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 22, 33, 11 }, intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 33, 11, 22 }, intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 33, 22, 11 }, intComparer));

         // unequal sequences but equal sets - repetition
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11 },         new int[] { 11, 11 },         intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11 },         new int[] { 11, 11, 11 },     intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22 },     new int[] { 11, 22, 22 },     intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22 },     new int[] { 11, 11, 22 },     intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 11, 22, 22, 33 }, intComparer));
         Assert.AreEqual(0, CompareUtil.CompareSet<int>(new int[] { 11, 22, 33 }, new int[] { 11, 22, 33, 33 }, intComparer));
      }
   }
}
