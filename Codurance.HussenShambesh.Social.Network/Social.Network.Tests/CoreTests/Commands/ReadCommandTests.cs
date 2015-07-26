namespace Social.Network.Tests.CoreTests.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Moq;
	using NUnit.Framework;
	using Core.Commands;
	using Core.Interfaces;
	using Models;

	[TestFixture]
	public class ReadCommandTests
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
		public void ReadUserPostsTest()
		{
			// act
			string input = "Bob";
			var user = new User { UserId = Guid.NewGuid(), Username = input };

			var output = new StringBuilder();
			output.AppendLine("Good game though. (1 minute(s) ago)");
			output.AppendLine("Damn! We lost! (2 minute(s) ago)");
			
			var messages = new List<Message>
			{
				new Message { Description = "Good game though.", PostedDt = DateTime.UtcNow.AddMinutes(-1)},
				new Message { Description = "Damn! We lost!", PostedDt = DateTime.UtcNow.AddMinutes(-2)}
			};

			_userManagerMock.Setup(x => x.GetUserByUsername(user.Username, true)).Returns(user);
			_messageManagerMock.Setup(x => x.GetMessages(user)).Returns(messages);

			var read = new Read(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = read.Perform(input);

			// assert
			Assert.AreEqual(actual, output.ToString());
			_messageManagerMock.Verify(x => x.GetMessages(user), Times.Once);
		}

		[Test]
		public void ReadInvalidUserPosts()
		{
			// act
			string input = "Alice";

			_userManagerMock.Setup(x => x.GetUserByUsername("Alice", true)).Throws(new NullReferenceException("Alice does not exists!"));

			var read = new Read(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = read.Perform(input);

			// assert
			Assert.AreEqual(actual, "Alice does not exists!");
			_messageManagerMock.Verify(x => x.GetMessages(It.IsAny<User>()), Times.Never);
		}
	}
}
