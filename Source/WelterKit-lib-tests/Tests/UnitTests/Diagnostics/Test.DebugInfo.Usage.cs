using System;
using WelterKit.ClassTools;
using WelterKit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.UnitTests.Diagnostics {
   [TestClass]
   public class Test_DebugInfo_Usage {
      [TestMethod]
      public void IDiagger_Covariance() {
         testFunction<TestEntity>(getDiagger<TestEntity>().Info);


         void testFunction<T>(Func<T, DebugInfoBase<T>> entityInfoFunc) {
            // do nothing - this just tests syntax compileability
         }

         IDiagInfoGetter<T> getDiagger<T>() {
            return ( IDiagInfoGetter<T> )TestEntity.ClassMeta.GetDiagger();
         }
      }


      [TestMethod]
      public void IDiagger_CallViaInterface() {
         var x = new TestEntity();
         ( ( IDiagInfoGetter<TestEntity> )TestEntity.ClassMeta.GetDiagger() ).Info(x);
      }



      private class TestEntity {
         public static readonly IDiagInfoGetter<TestEntity> Diag = new Diagger();

         private class Diagger : IDiagInfoGetter<TestEntity> {
            public DebugInfoBase<TestEntity> Info(TestEntity obj) => TestEntity.GetDebugInfo(obj);
         }


         private static Meta _meta;
         public static Meta ClassMeta => _meta ??= new Meta();

         public class Meta : ClassMetaBase, IHasDiagInfoGetter<TestEntity> {
            public IDiagInfoGetter<TestEntity> GetDiagger() => TestEntity.Diag;
         }


         #region Diagnostics
         public static DebugInfoBase<TestEntity> GetDebugInfo(TestEntity obj) => new DebugInfo(obj);
         private class DebugInfo : SimpleDebugInfoBase<TestEntity> {
            public DebugInfo(TestEntity obj) : base(obj) { }
            protected override string GetContents() => "[test entity]";
         }

         #endregion Diagnostics
      }
   }
}
