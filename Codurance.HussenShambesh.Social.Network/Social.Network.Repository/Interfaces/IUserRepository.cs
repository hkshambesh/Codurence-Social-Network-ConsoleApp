namespace Social.Network.Repository.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Models;

	public interface IUserRepository : IDisposable
	{
		void Add(User user);
		void AddChild(User parent, User child);
		User Get(Guid? userId = null, string username = null); 
		IEnumerable<User> GetChildren(User user);
	}
}