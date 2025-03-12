using System;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.AbstractDataTypes.Trees {
   public class Tree<T> : TreeNode<T> {
      public Tree(T value, List<TreeNode<T>>    nodes) : base(value, nodes) { }
      public Tree(T value, params TreeNode<T>[] nodes) : base(value, nodes.ToList()) { }

      /// <summary>
      /// Convert subtree to full tree.
      /// </summary>
      public Tree(TreeNode<T> subtree) : base(subtree) { }
   }
}
