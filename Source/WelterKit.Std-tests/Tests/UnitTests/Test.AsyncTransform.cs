using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WelterKit.Std;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WelterKit_Tests.Tests.UnitTests {
   [TestClass]
   public class Test_AsyncTransform {
      [TestMethod]
      public void InvokeAsSyncBlocking_Action_Sample() {
         var list = new List<int>();
         Func<Task> asyncAction = () => addToListAsync(list, 42);
         asyncAction.InvokeAsSyncBlocking();
         CollectionAssert.AreEqual(new List<int>() { 42 }, list);
      }


      [TestMethod]
      public void InvokeAsSyncBlocking_Func_Sample() {
         Func<Task<int>> asyncFunc = () => getValueAsync(42);
         int actual = asyncFunc.InvokeAsSyncBlocking();
         Assert.AreEqual(42, actual);
      }


      // TODO
      // [TestMethod]
      // public async Task SyncToAsync_Action_Sample() {
      //    var list = new List<int>();
      //    Action syncAction = () => addToList(list, 42);
      //    await syncAction.InvokeAsAsync();
      //    CollectionAssert.AreEqual(new List<int>() { 42 }, list);
      // }


      // TODO
      // [TestMethod]
      // public async Task SyncToAsync_Func_Sample() {
      //    Func<int> syncFunc = () => getValue(42);
      //    int actual = await syncFunc.InvokeAsAsync();
      //    Assert.AreEqual(42, actual);
      // }


      private int getValue(int value)
         => value;


      private void addToList(List<int> list, int value) {
         list.Add(value);
      }


      private static async Task addToListAsync(List<int> list, int value) {
         list.Add(value);
         await Console.Out.WriteLineAsync("Writing asynchronously.")
                      .ConfigureAwait(false);
      }


      private static async Task<int> getValueAsync(int value) {
         await Console.Out.WriteLineAsync("Writing asynchronously.")
                      .ConfigureAwait(false);
         return value;
      }
   }
}
