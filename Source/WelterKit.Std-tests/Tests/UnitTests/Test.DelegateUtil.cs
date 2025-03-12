using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WelterKit.Std.StaticUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   [TestCategory("Unit")]
   public class DelegateUtilTests {
      [TestMethod]
      public void Wrap_Func_Sample1() {
         var list = new List<string>();

         int result = ( ( Func<int> )func )
              .Wrap(before, after);
         Assert.AreEqual(42, result);
         CollectionAssert.AreEqual(new List<string>() { "before", "after:42" }, list);

         int func() => 42;
         void before()       { list.Add("before"); }
         void after(int num) { list.Add("after:" + num); }
      }


      [TestMethod]
      public void Wrap_Func_Sample2() {
         var list = new List<string>();

         int result = ( ( Func<int> )func )
              .Wrap(before, after);
         Assert.AreEqual(42, result);
         CollectionAssert.AreEqual(new List<string>() { "before", "after" }, list);

         int func() => 42;
         void before() { list.Add("before"); }
         void after()  { list.Add("after"); }
      }


      [TestMethod]
      public async Task WrapAsync_Func_sample1() {
         const int n = 42,
                   expectedResult = 267914296;
         var list = new List<string>();

         int result = await ( ( Func<Task<int>> )func )
                           .WrapAsync(before, after);
         Assert.AreEqual(expectedResult, result);
         CollectionAssert.AreEqual(new List<string>() { "before", $"after:{expectedResult}" }, list);

         async Task<int> func() => await computeFibonacciAsync(n);
         void before()       { list.Add("before"); }
         void after(int num) { list.Add("after:" + num); }
      }


      [TestMethod]
      public async Task WrapAsync_Func_sample2() {
         const int n = 42,
                   expectedResult = 267914296;
         var list = new List<string>();

         int result = await ( ( Func<Task<int>> )func )
                           .WrapAsync(before, after);
         Assert.AreEqual(expectedResult, result);
         CollectionAssert.AreEqual(new List<string>() { "before", "after" }, list);

         async Task<int> func() => await computeFibonacciAsync(n);
         void before() { list.Add("before"); }
         void after()  { list.Add("after"); }
      }


      [TestMethod]
      public void Wrap_Action_Sample() {
         var list = new List<string>();

         ( ( Action )action )
              .Wrap(before, after);
         CollectionAssert.AreEqual(new List<string>() { "before", "in", "after" }, list);

         void action() { list.Add("in"); }
         void before() { list.Add("before"); }
         void after()  { list.Add("after"); }
      }


      [TestMethod]
      public async Task WrapAsync_Action_Sample() {
         const int n = 42;
         var list = new List<string>();

         await ( ( Func<Task> )action )
              .WrapAsync(before, after);
         CollectionAssert.AreEqual(new List<string>() { "before", "in", "after" }, list);

         async Task action() {
            list.Add("in");
            await computeFibonacciAsync(n);
         }
         void before() { list.Add("before"); }
         void after()  { list.Add("after"); }
      }


      //===
      private async Task<int> computeFibonacciAsync(int n)
         => await Task.Run(() => naiveFib(n));


      private int naiveFib(int n)
         => n switch
            {
               0 => 0,
               1 => 1,
               _ => naiveFib(n - 1) + naiveFib(n - 2)
            };
   }
}
