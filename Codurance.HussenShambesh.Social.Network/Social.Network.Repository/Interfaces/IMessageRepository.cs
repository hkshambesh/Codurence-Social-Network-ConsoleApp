namespace Social.Network.Repository.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Models;

	public interface IMessageRepository : IDisposable
	{
		void Add(Message message);
		void AddChild(Message message);
		Message Get(Guid? userId = null);
		IEnumerable<Message> GetAll(User user);
	}
}