using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Social.Network.Core.Commands;
using Social.Network.Core.Interfaces;
using Social.Network.Models;

namespace Social.Network.Tests.CoreTests.Commands
{
	[TestFixture]
	public class FollowCommandTests
	{
		private Mock<IUserManager> _userManagerMock;

		[TestFixtureSetUp]
		public void Setup()
		{
			_userManagerMock = new Mock<IUserManager>();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_userManagerMock = null;
		}

		[Test]
		public void FollowMultipleNewUserTest()
		{
			// act
			string input = "Charlie follows Alice";
			var parent = new User { Username = "Charlie" };
			var child = new User { Username = "Alice" };

			_userManagerMock.Setup(x => x.GetUserByUsername(parent.Username, true)).Returns(parent);
			_userManagerMock.Setup(x => x.GetUserByUsername(child.Username, true)).Returns(child);
			_userManagerMock.Setup(x => x.CheckFollowerExists(parent, child));
			_userManagerMock.Setup(x => x.FollowUser(parent, child));

			var follow = new Follow(_userManagerMock.Object);

			// actual
			var actual = follow.Perform(input);

			// assert
			Assert.AreEqual(actual, string.Empty);
			_userManagerMock.Verify(x => x.FollowUser(parent, child), Times.Once);
		}

		[Test]
		public void FollowAlreadyFollowedUserTest()
		{
			// act
			string input = "Charlie follows Alice";

			var parent = new User { Username = "Charlie" };
			var child = new User { Username = "Alice" };
			var children = new List<User> { child };


			_userManagerMock.Setup(x => x.GetUserByUsername(parent.Username, true)).Returns(parent);
			_userManagerMock.Setup(x => x.GetUserByUsername(child.Username, true)).Returns(child);
			_userManagerMock.Setup(x => x.CheckFollowerExists(parent, child)).Throws(new Exception("Alice already followed!"));

			var follow = new Follow(_userManagerMock.Object);

			// actual
			var actual = follow.Perform(input);

			// assert
			Assert.AreEqual(actual, "Alice already followed!");
		}

		[Test]
		public void FollowInvalidUserTest()
		{
			// act
			string input = "Charlie follows Alice";

			_userManagerMock.Setup(x => x.GetUserByUsername("Charlie", true)).Returns(new User());
			_userManagerMock.Setup(x => x.GetUserByUsername("Alice", true)).Throws(new NullReferenceException("Alice does not exists!"));

			var follow = new Follow(_userManagerMock.Object);

			// actual
			var actual = follow.Perform(input);

			// assert
			Assert.AreEqual(actual, "Alice does not exists!");
		}

		[Test]
		public void InvalidParentUserTest()
		{
			// act
			string input = "Charlie follows Alice";

			_userManagerMock.Setup(x => x.GetUserByUsername("Charlie", true)).Throws(new NullReferenceException("Charlie does not exists!"));

			var follow = new Follow(_userManagerMock.Object);

			// actual
			var actual = follow.Perform(input);

			// assert
			Assert.AreEqual(actual, "Charlie does not exists!");
		}
	}
}
