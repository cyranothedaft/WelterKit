using System;
using WelterKit.AbstractDataTypes;
using WelterKit.AbstractDataTypes.Trees;
using WelterKit.Functional;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WelterKit_Tests.UnitTests.AbstractDataTypes.Trees
{
   [TestClass]
   public class Test_Tree
   {
      [TestMethod]
      public void Children_Sample()
      {
         Tree<int> tree = getTestTree();

         Assert.AreEqual(3, tree.Children.Count);
         Assert.AreEqual(2, tree.Children[0].Children.Count);
         Assert.AreEqual(2, tree.Children[1].Children.Count);
         Assert.AreEqual(0, tree.Children[2].Children.Count);
      }


      [TestMethod]
      public void AddChild_Sample()
      {
         Tree<int> tree = getTestTree();
         var newNode = tree.Children[1].AddChild(42);

         Assert.AreEqual(3,  tree.Children[1].Children.Count);
         Assert.AreEqual(42, newNode.Value);
         Assert.AreEqual(5,  ( ( TreeNode<int> )( Some<TreeNode<int>> )newNode.GetParent() ).Value);
      }


      [TestMethod]
      public void FindFirstByValue_Sample()
      {
         Tree<int> tree = getTestTree();

         Assert.AreEqual(5, tree.FindFirstByValue(5)
                                .Map(node => node.Value)
                                .Reduce(0));
         Assert.IsTrue(tree.FindFirstByValue(99) is None<TreeNode<int>>);
      }


      [TestMethod]
      public void Traverse_Sample()
      {
         Tree<int> tree = getTestTree();

         int sum = 0;
         tree.Traverse(node => sum += node.Value);
         Assert.AreEqual(1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10, sum);
      }


      [TestMethod]
      public void Traverse_WithDepth_Sample()
      {
         Tree<int> tree = getTestTree();

         int sum = 0;
         tree.Traverse((node, depth) =>
         {
            sum += depth;
         });
         Assert.AreEqual(0 + 1 + 2 + 2 + 1 + 2 + 3 + 3 + 2 + 1, sum);
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
