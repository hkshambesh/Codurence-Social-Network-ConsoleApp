namespace Social.Network.Repository
{
	using System;
	using System.Collections.Generic;
	using Models;
	using BinaryTree;
	using Interfaces;

	/// <summary>
	/// class to manage the message repository functions
	/// </summary>
	public class MessageRepository : IMessageRepository
	{
		#region local members

		private Dictionary<Guid, Tree<Message>> _userMessageTreeDictionary;
		private Tree<Message> _messageTree;

		#endregion

		#region constructors

		public MessageRepository()
		{
			_userMessageTreeDictionary = new Dictionary<Guid, Tree<Message>>();
		}

		#endregion

		#region public methods

		public void Dispose()
		{
			this._messageTree = null;
			this._userMessageTreeDictionary = null;
		}

		public void Add(Message message)
		{
			if (_userMessageTreeDictionary.ContainsKey(message.UserId))
			{
				throw new Exception("User already exists.");
			}

			this._messageTree = new Tree<Message>(message);
			_userMessageTreeDictionary.Add(message.UserId, _messageTree);
		}

		public void AddChild(Message message)
		{
			if (!_userMessageTreeDictionary.ContainsKey(message.UserId))
			{
				throw new Exception("Parent message user does not exist");
			}

			this._messageTree = _userMessageTreeDictionary[message.UserId];

			this._messageTree.Root.AddChild(new TreeNode<Message>(message));
		}

		public Message Get(Guid? userId = null)
		{
			if (userId != null)
			{
				if (_userMessageTreeDictionary.ContainsKey(userId.Value))
				{
					return _userMessageTreeDictionary[userId.Value].Root.Value;
				}
			}

			return null;
		}

		public IEnumerable<Message> GetAll(User user)
		{
			if (!_userMessageTreeDictionary.ContainsKey(user.UserId))
			{
				throw new Exception("User does not exist");
			}

			_messageTree = _userMessageTreeDictionary[user.UserId];

			var children = new List<Message>();

			for (var i = 0; i < _messageTree.Root.ChildrenCount; i++)
			{
				children.Add(_messageTree.Root.GetChild(i).Value);
			}

			return children;
		}

		#endregion
	}
}
