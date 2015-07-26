namespace Social.Network.Core.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Models;
	using Interfaces;
	using Repository.Interfaces;

	public class MessageManager : IMessageManager
	{
		#region local members

		private readonly IMessageRepository _messageRepository;

		#endregion

		#region constructors

		public MessageManager(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		#endregion

		#region public methods

		public void AddMessage(Guid userId, string postMessage)
		{
			Message userMessage = _messageRepository.Get(userId);

			if (userMessage == null)
			{
				userMessage = new Message
				{
					UserId = userId
				};

				_messageRepository.Add(userMessage);
			}

			var message = new Message
			{
				MessageId = Guid.NewGuid(),
				Description = postMessage,
				PostedDt = DateTime.UtcNow,
				UserId = userMessage.UserId
			};

			_messageRepository.AddChild(message);
		}

		public IEnumerable<Message> GetMessages(User user)
		{
			return _messageRepository.GetAll(user).OrderByDescending(x => x.PostedDt);
		}

		#endregion
	}
}
