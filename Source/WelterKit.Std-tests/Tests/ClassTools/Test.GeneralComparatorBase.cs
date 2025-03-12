using System;
using System.Collections.Generic;
using WelterKit.Std.ClassTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit.Std_Tests.Tests.ClassTools {
   [TestClass]
   public class Test_GeneralComparatorBase {
      [TestMethod]
      public void IEqualityComparer_Sample() {
         testFunction<TestEntity>(getComparer<TestEntity>().Equals);


         void testFunction<T>(Func<T, T, bool> entityEqualsFunc) {
            // do nothing - this just tests syntax compileability
         }

         IEqualityComparer<T> getComparer<T>() where T : TooledClassBase {
            return ( IEqualityComparer<T> )TestEntity.Comparer;
         }
      }



      private class TestEntity : TooledClassBase {
         public static readonly GeneralComparatorBase<TestEntity> Comparer = new TestEntityComparer();

         private class TestEntityComparer : GeneralComparatorBase<TestEntity> {
            protected override bool IsNullable {
               get { throw new NotImplementedException(); }
            }

            protected override IEnumerable<Func<TestEntity, TestEntity, int>> GetOrderedCompareOps() {
               throw new NotImplementedException();
            }
         }
      }

   }
}
