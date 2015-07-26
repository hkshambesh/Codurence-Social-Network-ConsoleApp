namespace Social.Network.Repository.BinaryTree
{
	using System;
	
	/// <summary>
	/// Class to implement the Tree Data Structure with tree nodes
	/// </summary>
	/// <typeparam name="T">Generic type of Tree class</typeparam>
	public class Tree<T>
	{
		#region local members

		private readonly TreeNode<T> _root;

		#endregion

		#region constructors

		public Tree(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("Cannot insert null value!");
			}

			this._root = new TreeNode<T>(value);
		}

		public Tree(T value, params Tree<T>[] children)
			: this(value)
		{
			foreach (Tree<T> child in children)
			{
				this._root.AddChild(child._root);
			}
		}

		#endregion

		#region properties

		public TreeNode<T> Root
		{
			get { return this._root; }
		}

		#endregion
	}
}
