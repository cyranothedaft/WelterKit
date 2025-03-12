using System;
using WelterKit.Std.AbstractDataTypes.Trees;
using WelterKit.Std.Functional;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests.AbstractDataTypes {
   [TestClass]
   public class Test_Tree {
      [TestMethod]
      public void Children_Sample() {
         Tree<int> tree = getTestTree();

         Assert.AreEqual(3, tree.Children.Count);
         Assert.AreEqual(2, tree.Children[0].Children.Count);
         Assert.AreEqual(2, tree.Children[1].Children.Count);
         Assert.AreEqual(0, tree.Children[2].Children.Count);
      }


      [TestMethod]
      public void AddChild_Sample() {
         Tree<int> tree = getTestTree();
         var newNode= tree.Children[1].AddChild(42);
         Assert.AreEqual(3,  tree.Children[1].Children.Count);
         Assert.AreEqual(42, newNode.Value);
         Assert.AreEqual(5,  ( ( TreeNode<int> )( Some<TreeNode<int>> )newNode.GetParent() ).Value);
      }


      private static Tree<int> getTestTree()
         => new Tree<int>(1,
                          new TreeNode<int>(2,
                                            new TreeNode<int>(3),
                                            new TreeNode<int>(4)),
                          new TreeNode<int>(5,
                                            new TreeNode<int>(6,
                                                              new TreeNode<int>(7),
                                                              new TreeNode<int>(8)),
                                            new TreeNode<int>(9)),
                          new TreeNode<int>(10));

   }
}
