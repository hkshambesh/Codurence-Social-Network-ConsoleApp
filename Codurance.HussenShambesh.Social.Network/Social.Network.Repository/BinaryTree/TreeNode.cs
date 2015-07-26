namespace Social.Network.Repository.BinaryTree
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// class to implement the TreeNode of a Tree data structure
	/// </summary>
	/// <typeparam name="T">Generic type of the TreeNode class</typeparam>
	public class TreeNode<T>
	{
		#region local members

		private readonly List<TreeNode<T>> _children;
		private bool _hasParent;

		#endregion

		#region constructors

		public TreeNode(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("Cannot insert null value!");
			}

			this.Value = value;
			this._children = new List<TreeNode<T>>();
		}

		#endregion

		#region properties

		public T Value { get; set; }

		public int ChildrenCount
		{
			get { return _children.Count; }
		}

		#endregion

		#region public methods

		public void AddChild(TreeNode<T> child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("Cannot insert null value!");
			}

			if (child._hasParent)
			{
				throw new ArgumentException("The node already has a parent!");
			}

			child._hasParent = true;
			_children.Add(child);
		}

		public TreeNode<T> GetChild(int index)
		{
			return this._children[index];
		}

		#endregion
	}
}