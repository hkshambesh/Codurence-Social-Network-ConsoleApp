namespace Social.Network.Tests.CoreTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Moq;
	using NUnit.Framework;
	using Core.Managers;
	using Models;
	using Repository.Interfaces;

	[TestFixture]
	public class UserManagerTests
	{
		private Mock<IUserRepository> _userRepositoryMock;

		[TestFixtureSetUp]
		public void Setup()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_userRepositoryMock = null;
		}

		[Test]
		[ExpectedException]
		public void GetUserByInvalidUsernameTest()
		{
			// act
			_userRepositoryMock.Setup(x => x.Get(null, null)).Returns((User) null);
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.GetUserByUsername("test", true);

			// assert
			// never reached
		}

		[Test]
		public void GetUserByValidUsernameTest()
		{
			// act
			_userRepositoryMock.Setup(x => x.Get(null, "test")).Returns(new User());
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.GetUserByUsername("test", true);

			// assert
			Assert.IsNotNull(actual);
		}

		[Test]
		[ExpectedException]
		public void GetUserByInvalidUserIdTest()
		{
			// act
			_userRepositoryMock.Setup(x => x.Get(null, null)).Returns((User)null);
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.GetUserByUserId(Guid.Empty, true);

			// assert
			// never reached
		}

		[Test]
		public void GetUserByValidUserIdTest()
		{
			// act
			Guid userId = Guid.NewGuid();

			_userRepositoryMock.Setup(x => x.Get(userId, null)).Returns(new User());
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.GetUserByUserId(userId, true);

			// assert
			Assert.IsNotNull(actual);
		}

		[Test]
		public void AddUserTest()
		{
			// act

			_userRepositoryMock.Setup(x => x.Add(It.IsAny<User>()));
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.AddUser("test");

			// assert
			Assert.AreEqual(actual.Username, "test");
		}

		[Test]
		public void FollowUserTest()
		{
			// act

			_userRepositoryMock.Setup(x => x.AddChild(It.IsAny<User>(), It.IsAny<User>()));
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			userManager.FollowUser(new User(), new User());

			// assert
			_userRepositoryMock.Verify(x => x.AddChild(It.IsAny<User>(), It.IsAny<User>()));
		}

		[Test]
		[ExpectedException]
		public void CheckFollowerExistsTest()
		{
			// act
			var parent = new User();
			var follower = new User { UserId = Guid.NewGuid() };
			var followers = new List<User> { follower };

			_userRepositoryMock.Setup(x => x.GetChildren(parent)).Returns(followers);
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			userManager.CheckFollowerExists(parent, follower);

			// assert
			// never reached
		}

		[Test]
		public void CheckFollowerNotExistsTest()
		{
			// act

			_userRepositoryMock.Setup(x => x.GetChildren(It.IsAny<User>())).Returns(new List<User>());
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			userManager.FollowUser(new User(), new User());

			// assert
			_userRepositoryMock.Verify(x => x.AddChild(It.IsAny<User>(), It.IsAny<User>()));
		}

		[Test]
		public void GetFollowersTest()
		{
			// act

			_userRepositoryMock.Setup(x => x.GetChildren(It.IsAny<User>())).Returns(new List<User>
			{
				new User(),
				new User()
			});
			var userManager = new UserManager(_userRepositoryMock.Object);

			// actual
			var actual = userManager.GetFollowers(new User());

			// assert
			Assert.AreEqual(actual.Count(), 2);
		}
	}
}
