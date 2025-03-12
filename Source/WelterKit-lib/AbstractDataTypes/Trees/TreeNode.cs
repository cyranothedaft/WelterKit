using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WelterKit.Functional;



namespace WelterKit.AbstractDataTypes.Trees {
   [DebuggerDisplay("{Value}+[{_children.Count}]")]
   public class TreeNode<T> : IEquatable<TreeNode<T>> {
      private Maybe<TreeNode<T>> _parent_ { get; }
      private readonly List<TreeNode<T>> _children;

      public T Value { get; }

      public IReadOnlyList<TreeNode<T>> Children => _children;
      public bool HasParent => _parent_ is Some<TreeNode<T>>;
      public bool HasChildren => _children.Any();


      private TreeNode(Maybe<TreeNode<T>> parent, T value, IEnumerable<TreeNode<T>> children) {
         _parent_   = parent;
         Value     = value;
         _children = children.Select(n => n.WithParent(this))
                             .ToList();
      }


      /// <summary>
      /// Copy constructor
      /// </summary>
      protected TreeNode(TreeNode<T> other)
            : this(other._parent_, other.Value, cloneChildList(other._children)) { }


      /// <summary>
      /// For parentless node
      /// </summary>
      public TreeNode(T value, List<TreeNode<T>> children)
            : this(None.Value, value, children) { }


      /// <summary>
      /// For internal or leaf node (i.e., node that has a parent).
      /// </summary>
      public TreeNode(TreeNode<T> parent, T value, IEnumerable<TreeNode<T>> children)
            : this(( Maybe<TreeNode<T>> )parent, value, children) { }


      public TreeNode(TreeNode<T> parent, T value, params TreeNode<T>[] children)
            : this(( Maybe<TreeNode<T>> )parent, value, children) { }


      /// <summary>
      /// Unspecified parent - used during construction.
      /// TODO: this is kind of a HACK; update code to enforce using language features
      /// </summary>
      public TreeNode(T value, IEnumerable<TreeNode<T>> children)
            : this(( Maybe<TreeNode<T>> )None.Value, value, children) { }


      /// <summary>
      /// Unspecified parent - used during construction.
      /// TODO: this is kind of a HACK; update code to enforce using language features
      /// </summary>
      public TreeNode(T value, params TreeNode<T>[] children)
            : this(value, ( IEnumerable<TreeNode<T>> )children) { }


      public TreeNode<T> WithParent(TreeNode<T> newParent)
         => new TreeNode<T>(newParent, this.Value, cloneChildList(_children));


      public Maybe<TreeNode<T>> GetParent()
         => _parent_;


      public TreeNode<T> AddChild(T valueOfChild, params TreeNode<T>[] childrenOfChild) {
         var newNode = new TreeNode<T>(this, valueOfChild, childrenOfChild);
         _children.Add(newNode);
         return newNode;
      }


      public TreeNode<T> AddChild(TreeNode<T> childNode) {
         childNode = childNode.WithParent(this);
         _children.Add(childNode);
         return childNode;
      }


      public Maybe<TreeNode<T>> FindFirstByValue(T value, IEqualityComparer<T>? comparer = null)
         => findFirstByValue(value, (x, y) => comparer?.Equals(x, y)
                                           ?? EqualityComparer<T>.Default.Equals(x, y));


      // TODO: optimize
      private Maybe<TreeNode<T>> findFirstByValue(T value, Func<T, T, bool> equalsFunc) {
         Maybe<TreeNode<T>> foundNodeOption = None.Value;
         Traverse(node => {
                     if ( foundNodeOption is None<TreeNode<T>> && equalsFunc(node.Value, value) )
                        foundNodeOption = node;
                  });
         return foundNodeOption;
      }


      public void Traverse(Action<TreeNode<T>> nodeAction) {
         Traverse(this, nodeAction);
      }


      public void Traverse(Action<TreeNode<T>, int> nodeAction) {
         traverse(this, 0, nodeAction);
      }


      public static void Traverse(TreeNode<T> node, Action<TreeNode<T>> nodeAction) {
         nodeAction(node);
         foreach ( TreeNode<T> child in node.Children )
            Traverse(child, nodeAction);
      }


      private static void traverse(TreeNode<T> node, int currentDepth, Action<TreeNode<T>, int> nodeAction) {
         nodeAction(node, currentDepth);
         foreach ( TreeNode<T> child in node.Children )
            traverse(child, currentDepth + 1, nodeAction);
      }


      private static List<TreeNode<T>> cloneChildList(List<TreeNode<T>> children)
         => new List<TreeNode<T>>(children);


      #region Equality

      public bool Equals(TreeNode<T>? other) {
         if ( ReferenceEquals(null, other) ) return false;
         if ( ReferenceEquals(this, other) ) return true;
         return _children.Equals(other._children)
             && _parent_.Equals(other._parent_)
             && EqualityComparer<T>.Default.Equals(Value, other.Value);
      }

      public override bool Equals(object? obj) {
         if ( ReferenceEquals(null, obj) ) return false;
         if ( ReferenceEquals(this, obj) ) return true;
         if ( obj.GetType() != this.GetType() ) return false;
         return Equals(( TreeNode<T> )obj);
      }

      public override int GetHashCode() => HashCode.Combine(_children, _parent_, Value);
      public static bool operator ==(TreeNode<T>? left, TreeNode<T>? right) => Equals(left, right);
      public static bool operator !=(TreeNode<T>? left, TreeNode<T>? right) => !Equals(left, right);

      #endregion Equality
   }
}
