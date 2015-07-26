namespace Social.Network.Tests.CoreTests.Commands
{
	using System;
	using Moq;
	using NUnit.Framework;
	using Core.Commands;
	using Core.Interfaces;
	using Models;

	[TestFixture]
	public class PostCommandTests
	{
		private Mock<IUserManager> _userManagerMock;
		private Mock<IMessageManager> _messageManagerMock;

		[TestFixtureSetUp]
		public void Setup()
		{
			_userManagerMock = new Mock<IUserManager>();
			_messageManagerMock = new Mock<IMessageManager>();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_userManagerMock = null;
			_messageManagerMock = null;
		}

		[Test]
		public void PerformPostNewMessageOnNewUserTest()
		{
			// act
			string input = "Alice -> I love the weather today";

			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "Alice"
			};

			_userManagerMock.Setup(x => x.GetUserByUsername(user.Username, false)).Returns((User) null);
			_userManagerMock.Setup(x => x.AddUser(user.Username)).Returns(user);

			_messageManagerMock.Setup(x => x.AddMessage(user.UserId, input));

			var perform = new Post(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = perform.Perform(input);

			// assert
			Assert.AreEqual(actual, string.Empty);
			_userManagerMock.Verify(x=>x.AddUser(It.IsAny<string>()), Times.Once);
			_messageManagerMock.Verify(x=>x.AddMessage(It.IsAny<Guid>(),It.IsAny<string>()), Times.AtLeastOnce);
		}

		[Test]
		public void PerformPostMessageOnExistingUserTest()
		{
			// act
			string input = "Bob	-> Good game though.";
			User user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "Bob"
			};

			Message userMessage = new Message
			{
				UserId = user.UserId
			};

			_userManagerMock.Setup(x => x.GetUserByUsername(user.Username, false)).Returns(user);
			_messageManagerMock.Setup(x => x.AddMessage(user.UserId, input));

			var perform = new Post(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = perform.Perform(input);

			// assert
			Assert.AreEqual(actual, string.Empty);
			_userManagerMock.Verify(x => x.AddUser(It.IsAny<string>()), Times.Never);
			_messageManagerMock.Verify(x => x.AddMessage(It.IsAny<Guid>(), It.IsAny<string>()), Times.AtLeastOnce);
		}
	}
}
