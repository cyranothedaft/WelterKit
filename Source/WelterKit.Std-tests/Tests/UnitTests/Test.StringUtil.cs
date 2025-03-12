using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Std.StaticUtilities;



namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_StringUtil {
      [TestMethod]
      public void Overwrite_sample() {
         Assert.AreEqual("12abc6789", "123456789".Overwrite("abc", 2));
      }


      [TestMethod]
      public void Overwrite_null() {
         Assert.ThrowsException<NullReferenceException>(() => StringUtil.Overwrite(null,        null,  2));
         Assert.ThrowsException<NullReferenceException>(() => StringUtil.Overwrite(null,        "abc", 2));
         Assert.ThrowsException<NullReferenceException>(() => StringUtil.Overwrite("123456789", null,  2));
      }
      // TODO: also test Overwrite with bad positions/indexes


      [TestMethod]
      public void Repeat_char_sample() {
         Assert.AreEqual("***", '*'.Repeat(3));
      }


      [TestMethod]
      public void Repeat_string_sample() {
         Assert.AreEqual("AbcAbcAbc", "Abc".Repeat(3));
      }


      [TestMethod]
      public void Repeat_char_valid_0() {
         Assert.AreEqual("", 'x'.Repeat(0));
      }


      [TestMethod]
      public void Repeat_char_valid_1() {
         Assert.AreEqual("x", 'x'.Repeat(1));
      }


      [TestMethod]
      public void Repeat_string_valid_0() {
         testFor0("x");
         testFor0("xx");
         testFor0("Abc");
         testFor0("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

         void testFor0(string s) {
            Assert.AreEqual("", s.Repeat(0));
         }
      }


      [TestMethod]
      public void Repeat_string_valid_1() {
         testFor1("x");
         testFor1("xx");
         testFor1("Abc");
         testFor1("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

         void testFor1(string s) {
            Assert.AreEqual(s, s.Repeat(1));
         }
      }


      [TestMethod]
      public void Repeat_valid_empty() {
         Assert.AreEqual("", "".Repeat(0));
         Assert.AreEqual("", "".Repeat(1));
         Assert.AreEqual("", "".Repeat(2));
         Assert.AreEqual("", "".Repeat(3));
         Assert.AreEqual("", "".Repeat(99));
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentNullException ))]
      public void Repeat_invalid_null() {
         StringUtil.Repeat(null, 2);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void Repeat_invalid_negativeCount() {
         "Abc".Repeat(-2);
      }


      [TestMethod]
      public void SplitAt_valid_empty() {
         Assert.AreEqual(( "", "" ), StringUtil.SplitAt("", 0));
      }


      [TestMethod]
      public void SplitAt_valid() {
         Assert.AreEqual(( "", "a" ), StringUtil.SplitAt("a", 0));
         Assert.AreEqual(( "a", "" ), StringUtil.SplitAt("a", 1));
         Assert.AreEqual(( "", "ab" ), StringUtil.SplitAt("ab", 0));
         Assert.AreEqual(( "a", "b" ), StringUtil.SplitAt("ab", 1));
         Assert.AreEqual(( "ab", "" ), StringUtil.SplitAt("ab", 2));
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentNullException ))]
      public void SplitAt_invalid_null() {
         StringUtil.SplitAt(null, 0);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange_1() {
         StringUtil.SplitAt("", -1);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange_2() {
         StringUtil.SplitAt("", 1);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange_3() {
         StringUtil.SplitAt("", 111);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange_4() {
         StringUtil.SplitAt("a", -1);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange_5() {
         StringUtil.SplitAt("a", 2);
      }


      [TestMethod]
      [ExpectedException(typeof( ArgumentOutOfRangeException ))]
      public void SplitAt_invalid_outofrange() {
         StringUtil.SplitAt("a", 222);
      }


      [TestMethod]
      public void StripWhitespace_nullempty() {
         Assert.AreEqual(null, StringUtil.StripWhitespace(null));
         Assert.AreEqual("", StringUtil.StripWhitespace(""));
      }


      [TestMethod]
      public void StripWhitespace_noWhitespace() {
         testStripWhitespace_idem("a");
         testStripWhitespace_idem("abc");
         testStripWhitespace_idem("abc123\\_-+xyz");
         testStripWhitespace_idem("\\_-+ÿ");
      }


      [TestMethod]
      public void StripWhitespace_noWhitespaceUnicode() {
         testStripWhitespace_idem("ĆƑǅɮ̶");
      }


      [TestMethod]
      public void StripWhitespace_onlyWhitespaceHomog() {
         for ( int i = 1; i <= 100; ++i ) Assert.AreEqual("", StringUtil.StripWhitespace(new string(' ', i)), $"{i} spaces");
         for ( int i = 1; i <= 100; ++i ) Assert.AreEqual("", StringUtil.StripWhitespace(new string('\t', i)), $"{i} tabs");
         for ( int i = 1; i <= 100; ++i ) Assert.AreEqual("", StringUtil.StripWhitespace(new string('\r', i)), $"{i} returns");
         for ( int i = 1; i <= 100; ++i ) Assert.AreEqual("", StringUtil.StripWhitespace(new string('\n', i)), $"{i} newlines");
      }


      [TestMethod]
      public void StripWhitespace_onlyWhitespaceHeterog() {
         char[] basicWhitespaceChars = { ' ', '\t', '\r', '\n' };
         foreach ( char a in basicWhitespaceChars )
         foreach ( char b in basicWhitespaceChars )
         foreach ( char c in basicWhitespaceChars )
         foreach ( char d in basicWhitespaceChars )
            testStripWhitespace("", "" + a + b + c + d);
      }


      [TestMethod]
      public void StripWhitespace_withWhitespace() {
         char[] allWhitespaceChars = { ' ', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v' };
         foreach ( char c in allWhitespaceChars ) {
            testStripWhitespace("a",  "a" + c);
            testStripWhitespace("a",  c + "a");
            testStripWhitespace("ab", "a" + c + "b");
         }
      }


      private static void testStripWhitespace_idem(string s) {
         testStripWhitespace(s, s);
      }

      private static void testStripWhitespace(string expected, string input) {
         Assert.AreEqual(expected, StringUtil.StripWhitespace(input),
                         "input: " + ByteUtil.ByteArrayToHexSpaced(Encoding.Unicode.GetBytes(input), " "));
      }


      [TestMethod]
      public void RemoveStart_blanks() {
         Assert.AreEqual(null, StringUtil.RemoveStart(null, null));
         Assert.AreEqual("", StringUtil.RemoveStart("", null));
         Assert.AreEqual("a", StringUtil.RemoveStart("a", null));
         Assert.AreEqual("abc", StringUtil.RemoveStart("abc", null));
         Assert.AreEqual("", StringUtil.RemoveStart("", ""));
         Assert.AreEqual("a", StringUtil.RemoveStart("a", ""));
         Assert.AreEqual("abc", StringUtil.RemoveStart("abc", ""));
         Assert.AreEqual(null, StringUtil.RemoveStart(null, "abc"));
      }


      [TestMethod]
      public void RemoveStart_na() {
         Assert.AreEqual("a", StringUtil.RemoveStart("a", "b"));
         Assert.AreEqual("a", StringUtil.RemoveStart("a", "abc"));
         Assert.AreEqual("abc", StringUtil.RemoveStart("abc", "xyz"));
      }


      [TestMethod]
      public void RemoveStart_all() {
         Assert.AreEqual("", StringUtil.RemoveStart("a", "a"));
         Assert.AreEqual("", StringUtil.RemoveStart("abc", "abc"));
      }


      [TestMethod]
      public void RemoveStart_valid() {
         Assert.AreEqual("bc", StringUtil.RemoveStart("abc", "a"));
         Assert.AreEqual("c", StringUtil.RemoveStart("abc", "ab"));
         Assert.AreEqual("_xyz", StringUtil.RemoveStart("abc_xyz", "abc"));
         Assert.AreEqual("_", StringUtil.RemoveStart(" _", " "));
         Assert.AreEqual(" ", StringUtil.RemoveStart("_ ", "_"));
      }


      [TestMethod]
      public void RemoveEnd_blanks() {
         Assert.AreEqual(null, StringUtil.RemoveEnd(null, null));
         Assert.AreEqual("", StringUtil.RemoveEnd("", null));
         Assert.AreEqual("a", StringUtil.RemoveEnd("a", null));
         Assert.AreEqual("abc", StringUtil.RemoveEnd("abc", null));
         Assert.AreEqual("", StringUtil.RemoveEnd("", ""));
         Assert.AreEqual("a", StringUtil.RemoveEnd("a", ""));
         Assert.AreEqual("abc", StringUtil.RemoveEnd("abc", ""));
         Assert.AreEqual(null, StringUtil.RemoveEnd(null, "abc"));
      }


      [TestMethod]
      public void RemoveEnd_na() {
         Assert.AreEqual("a", StringUtil.RemoveEnd("a", "b"));
         Assert.AreEqual("c", StringUtil.RemoveEnd("c", "abc"));
         Assert.AreEqual("abc", StringUtil.RemoveEnd("abc", "xyz"));
      }


      [TestMethod]
      public void RemoveEnd_all() {
         Assert.AreEqual("", StringUtil.RemoveEnd("a", "a"));
         Assert.AreEqual("", StringUtil.RemoveEnd("abc", "abc"));
      }


      [TestMethod]
      public void RemoveEnd_valid() {
         Assert.AreEqual("ab", StringUtil.RemoveEnd("abc", "c"));
         Assert.AreEqual("a", StringUtil.RemoveEnd("abc", "bc"));
         Assert.AreEqual("abc_", StringUtil.RemoveEnd("abc_xyz", "xyz"));
         Assert.AreEqual(" ", StringUtil.RemoveEnd(" _", "_"));
         Assert.AreEqual("_", StringUtil.RemoveEnd("_ ", " "));
      }


      [TestMethod]
      public void Until_blanks() {
         Assert.IsNull(StringUtil.Until(null, null));
         Assert.IsNull(StringUtil.Until(null, "x"));
         Assert.AreEqual("x", StringUtil.Until("x", null));

         Assert.AreEqual("", StringUtil.Until("", ""));
         Assert.AreEqual("", StringUtil.Until("", "x"));
         Assert.AreEqual("x", StringUtil.Until("x", ""));
      }


      [TestMethod]
      public void Until_example() {
         Assert.AreEqual("FirstPart", StringUtil.Until("FirstPart.TheRest", "."));
      }


      [TestMethod]
      public void Until_found_entireString() {
         Assert.AreEqual("", StringUtil.Until("x", "x"));
         Assert.AreEqual("", StringUtil.Until("xyz", "xyz"));
      }


      [TestMethod]
      public void Until_found_beginning() {
         Assert.AreEqual("", StringUtil.Until("x0", "x"));
         Assert.AreEqual("", StringUtil.Until("x0000000000000000000000000", "x"));
      }


      [TestMethod]
      public void Until_found_middle() {
         Assert.AreEqual("x", StringUtil.Until("xyz", "y"));
         Assert.AreEqual("x", StringUtil.Until("x10000000000000", "1"));
         Assert.AreEqual("x", StringUtil.Until("x11110000000000000", "1111"));
         Assert.AreEqual("x000000000000", StringUtil.Until("x00000000000010", "1"));
         Assert.AreEqual("x000000000000", StringUtil.Until("x00000000000011110", "1111"));
      }


      [TestMethod]
      public void Until_found_end() {
         Assert.AreEqual("0", StringUtil.Until("0x", "x"));
         Assert.AreEqual("0000000000000000000000000", StringUtil.Until("0000000000000000000000000x", "x"));
      }


      [TestMethod]
      public void Until_found_partMulti() {
         Assert.AreEqual("x", StringUtil.Until("x100100100", "1"));
         Assert.AreEqual("x", StringUtil.Until("x100100100", "100"));
      }


      [TestMethod]
      public void Until_notFound() {
         Assert.AreEqual("x", StringUtil.Until("x", "a"));
         Assert.AreEqual("x", StringUtil.Until("x", "abc"));
         Assert.AreEqual("abc", StringUtil.Until("abc", "x"));
         Assert.AreEqual("abc", StringUtil.Until("abc", "xyz"));
      }


      [TestMethod]
      public void Until_notFound_caseDiff() {
         Assert.AreEqual("xyz", StringUtil.Until("xyz", "X"));
         Assert.AreEqual("xyz", StringUtil.Until("xyz", "Y"));
         Assert.AreEqual("xyz", StringUtil.Until("xyz", "Z"));
         Assert.AreEqual("XYZ", StringUtil.Until("XYZ", "x"));
         Assert.AreEqual("XYZ", StringUtil.Until("XYZ", "y"));
         Assert.AreEqual("XYZ", StringUtil.Until("XYZ", "z"));
      }
   }
}
