namespace Social.Network.Core.Managers
{
	using System;
	using System.Linq;
	using Repository.Interfaces;
	using System.Collections.Generic;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to manage user functions
	/// </summary>
	public class UserManager : IUserManager
	{
		#region local members

		private readonly IUserRepository _userRepository;

		#endregion

		#region constructors

		public UserManager(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		#endregion

		#region public methods

		public User GetUserByUsername(string username, bool required)
		{
			User user = _userRepository.Get(username: username);

			if (required)
			{
				if (user == null)
				{
					throw new NullReferenceException(username + " does not exists!");
				}
			}

			return user;
		}

		public User GetUserByUserId(Guid userId, bool required)
		{
			User user = _userRepository.Get(userId: userId);

			if (required)
			{
				if (user == null)
				{
					throw new NullReferenceException(userId + " userId does not exists!");
				}
			}

			return user;
		}

		public User AddUser(string username)
		{
			User user = new User
			{
				UserId = Guid.NewGuid(),
				Username = username
			};

			_userRepository.Add(user);

			return user;
		}

		public void FollowUser(User parent, User child)
		{
			_userRepository.AddChild(parent, child);
		}

		public void CheckFollowerExists(User parent, User follower)
		{
			User user = _userRepository.GetChildren(parent).FirstOrDefault(x => x.UserId.Equals(follower.UserId));

			if (user != null)
			{
				throw new NullReferenceException(follower + " already followed!");
			}
		}

		public IEnumerable<User> GetFollowers(User user)
		{
			return _userRepository.GetChildren(user);
		}

		#endregion
	}
}