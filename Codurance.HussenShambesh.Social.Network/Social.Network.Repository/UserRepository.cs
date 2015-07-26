namespace Social.Network.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Models;
	using BinaryTree;
	using Interfaces;

	/// <summary>
	/// class to manage user repository functions
	/// </summary>
	public class UserRepository : IUserRepository
	{
		#region local members

		private Dictionary<Guid, Tree<User>> _userTreeDictionary;
		private Tree<User> _userTree;

		#endregion

		#region constructors

		public UserRepository()
		{
			_userTreeDictionary = new Dictionary<Guid, Tree<User>>();
		}

		#endregion

		#region public methods

		public void Add(User user)
		{
			if (_userTreeDictionary.ContainsKey(user.UserId))
			{
				throw new Exception("User already exists.");
			}

			_userTree = new Tree<User>(user);
			_userTreeDictionary.Add(user.UserId, _userTree);
		}

		public void AddChild(User parent, User child)
		{
			if (!_userTreeDictionary.ContainsKey(parent.UserId))
			{
				throw new Exception("Parent user does not exist");
			}

			_userTree = _userTreeDictionary[parent.UserId];

			_userTree.Root.AddChild(new TreeNode<User>(child));
		}

		public User Get(Guid? userId = null, string username = null)
		{
			if (userId != null)
			{
				if (_userTreeDictionary.ContainsKey(userId.Value))
				{
					return _userTreeDictionary[userId.Value].Root.Value;
				}
			}
			else if (username != null)
			{
				if (_userTreeDictionary.Any())
				{
					var found = _userTreeDictionary.FirstOrDefault(x => x.Value.Root.Value.Username.Equals(username));

					if (found.Value != null)
					{
						return found.Value.Root.Value;
					}
				}
			}
			
			return null;
		}

		public IEnumerable<User> GetChildren(User user)
		{
			if (!_userTreeDictionary.ContainsKey(user.UserId))
			{
				throw new Exception("User does not exist");
			}

			_userTree = _userTreeDictionary[user.UserId];

			var children = new List<User>();

			for (var i = 0; i < _userTree.Root.ChildrenCount; i++)
			{
				children.Add(_userTree.Root.GetChild(i).Value);
			}

			return children;
		}

		public void Dispose()
		{
			this._userTree = null;
			this._userTreeDictionary = null;
		}

		#endregion
	}
}