namespace Social.Network.Core.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Models;

	public interface IMessageManager
	{
		void AddMessage(Guid userId, string postMessage);
		IEnumerable<Message> GetMessages(User user);
	}
}