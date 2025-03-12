using System;
using WelterKit;
using WelterKit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_BasedFilePath_Comparator {
      [TestMethod]
      public void Compare_null() {
         Assert.AreEqual(0, BasedFilePath.Comparer.Compare(null, null));
         Assert.AreEqual(-1, BasedFilePath.Comparer.Compare(null, new BasedFilePath("a","b")));
         Assert.AreEqual(1, BasedFilePath.Comparer.Compare(new BasedFilePath("a", "b"), null));
      }


      [TestMethod]
      public void Compare_equal() {
         testEqual(new BasedFilePath("", ""));
         testEqual(new BasedFilePath("a", ""));
         testEqual(new BasedFilePath("", "b"));
         testEqual(new BasedFilePath("abc", ""));
         testEqual(new BasedFilePath("", "abc"));
         testEqual(new BasedFilePath("abc", "abc"));

         void testEqual(BasedFilePath basedFilePath) {
            Assert.AreEqual(0, BasedFilePath.Comparer.Compare(basedFilePath, basedFilePath), DiagStr.InfoShort(basedFilePath));
         }
      }


      [TestMethod]
      public void Compare_unequal() {
         testUnequal(new BasedFilePath("", ""), new BasedFilePath("", "a"));
         testUnequal(new BasedFilePath("", ""), new BasedFilePath("a", ""));
         testUnequal(new BasedFilePath("", "a"), new BasedFilePath("a", "a"));
         testUnequal(new BasedFilePath("", "a"), new BasedFilePath("", "ab"));
         testUnequal(new BasedFilePath("a", "a"), new BasedFilePath("a", "ab"));
         testUnequal(new BasedFilePath("a", "a"), new BasedFilePath("ab", "ab"));
         testUnequal(new BasedFilePath("", "abc xyy"), new BasedFilePath("", "abc xyz"));
         testUnequal(new BasedFilePath("", "abc xyy"), new BasedFilePath("a", "abc xyz"));
         testUnequal(new BasedFilePath("abc xyy", ""), new BasedFilePath("abc xyz", ""));
         testUnequal(new BasedFilePath("abc xyz", "abc xyy"), new BasedFilePath("abc xyz", "abc xyz"));

         void testUnequal(BasedFilePath lesser, BasedFilePath greater) {
            Assert.AreEqual(-1, BasedFilePath.Comparer.Compare(lesser, greater), $"{DiagStr.InfoShort(lesser)} =?= {DiagStr.InfoShort(greater)}");
            Assert.AreEqual(1, BasedFilePath.Comparer.Compare(greater, lesser), $"{DiagStr.InfoShort(greater)} =?= {DiagStr.InfoShort(lesser)}");
         }
      }


      [TestMethod]
      public void Equals_equal() {
         testEqual(new BasedFilePath(null, null));
         testEqual(new BasedFilePath("", ""));
         testEqual(new BasedFilePath("a", ""));
         testEqual(new BasedFilePath("", "b"));
         testEqual(new BasedFilePath("abc", ""));
         testEqual(new BasedFilePath("", "abc"));
         testEqual(new BasedFilePath("abc", "abc"));

         void testEqual(BasedFilePath basedFilePath) {
            Assert.IsTrue(BasedFilePath.Comparer.Equals(basedFilePath, basedFilePath), DiagStr.InfoShort(basedFilePath));
         }
      }


      [TestMethod]
      public void Equals_unequal() {
         testUnequal(new BasedFilePath("", ""), new BasedFilePath("", "a"));
         testUnequal(new BasedFilePath("", ""), new BasedFilePath("a", ""));
         testUnequal(new BasedFilePath("", "a"), new BasedFilePath("", "ab"));
         testUnequal(new BasedFilePath("a", "a"), new BasedFilePath("a", "ab"));
         testUnequal(new BasedFilePath("a", "a"), new BasedFilePath("ab", "ab"));
         testUnequal(new BasedFilePath("", "abc xyy"), new BasedFilePath("", "abc xyz"));
         testUnequal(new BasedFilePath("", "abc xyy"), new BasedFilePath("a", "abc xyz"));
         testUnequal(new BasedFilePath("abc xyy", ""), new BasedFilePath("abc xyz", ""));
         testUnequal(new BasedFilePath("abc xyz", "abc xyy"), new BasedFilePath("abc xyz", "abc xyz"));

         void testUnequal(BasedFilePath left, BasedFilePath right) {
            Assert.IsFalse(BasedFilePath.Comparer.Equals(left, right), $"{DiagStr.InfoShort(left)} =?= {DiagStr.InfoShort(right)}");
            Assert.IsFalse(BasedFilePath.Comparer.Equals(right, left), $"{DiagStr.InfoShort(right)} =?= {DiagStr.InfoShort(left)}");
         }
      }
   }
}
