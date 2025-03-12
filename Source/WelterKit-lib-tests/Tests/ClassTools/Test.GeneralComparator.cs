using System;
using System.Collections.Generic;
using WelterKit.ClassTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WelterKit_Tests.Tests.ClassTools {
   [TestClass]
   public class Test_GeneralComparator {
      [TestMethod]
      public void Sample() {
         HashSet<string> set = new HashSet<string>(new GeneralComparator<string>(
                                  equals_secondCharacter, getHashCode_secondCharacter))
                               { "aZb", "xAy" };
         Assert.IsTrue(set.Contains("aAa"));

         bool equals_secondCharacter(string str1, string str2)
            => str1[1] == str2[1];

         int getHashCode_secondCharacter(string str)
            => str[1].GetHashCode();
      }
   }
}
