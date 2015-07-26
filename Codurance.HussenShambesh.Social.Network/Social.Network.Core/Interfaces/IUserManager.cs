namespace Social.Network.Core.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Models;

	public interface IUserManager
	{
		User GetUserByUsername(string username, bool required);
		User GetUserByUserId(Guid userId, bool required);
		User AddUser(string username);
		void FollowUser(User parent, User child);
		void CheckFollowerExists(User parent, User follower);
		IEnumerable<User> GetFollowers(User user);
	}
}