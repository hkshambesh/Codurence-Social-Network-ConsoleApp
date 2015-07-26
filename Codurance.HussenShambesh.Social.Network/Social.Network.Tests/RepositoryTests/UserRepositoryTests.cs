namespace Social.Network.Tests.RepositoryTests
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using Models;
	using Repository;
	using Repository.Interfaces;

	[TestFixture]
	public class UserRepositoryTests
	{
		private IUserRepository _userRepository;

		[TestFixtureSetUp]
		public void Setup()
		{
			_userRepository = new UserRepository();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_userRepository.Dispose();
		}

		[Test]
		public void AddUserTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "test"
			}; 

			_userRepository.Add(user);

			// actual
			var actual = _userRepository.Get(userId: user.UserId);

			// assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.UserId, user.UserId);
		}

		[Test]
		public void GetUserByUsernameTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "test"
			};

			_userRepository.Add(user);

			// actual
			var actual = _userRepository.Get(username: user.Username);

			// assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.Username, user.Username);
		}

		[Test]
		[ExpectedException]
		public void AddDubplicateUserTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "test"
			};

			_userRepository.Add(user);
			_userRepository.Add(user);

			// actual
			var actual = _userRepository.Get();

			// assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.UserId, user.UserId);
		}

		[Test]
		public void AddUserChildrenTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "test"
			};

			var child = new User
			{
				UserId = Guid.NewGuid(),
				Username = "child"
			};

			_userRepository.Add(user);
			_userRepository.AddChild(user, child);

			// actual
			var actual = _userRepository.GetChildren(user).ToList();

			// assert
			Assert.AreEqual(actual.First(x => x.UserId.Equals(child.UserId)).UserId, child.UserId);
			Assert.AreEqual(actual.Count(), 1);
		}

		[Test]
		public void AddUserTwoChildrenTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "test"
			};

			var child1 = new User
			{
				UserId = Guid.NewGuid(),
				Username = "child1"
			};

			var child2 = new User
			{
				UserId = Guid.NewGuid(),
				Username = "child2"
			};

			_userRepository.Add(user);
			_userRepository.AddChild(user, child1);
			_userRepository.AddChild(user, child2);

			// actual
			var actual = _userRepository.GetChildren(user).ToList();

			// assert
			Assert.AreEqual(actual.First(x => x.UserId.Equals(child2.UserId)).UserId, child2.UserId);
			Assert.AreEqual(actual.Count(),2);
		}
	}
}
