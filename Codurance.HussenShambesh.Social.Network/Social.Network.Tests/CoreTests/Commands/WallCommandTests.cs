using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Social.Network.Core.Commands;
using Social.Network.Core.Interfaces;
using Social.Network.Models;

namespace Social.Network.Tests.CoreTests.Commands
{
	[TestFixture]
	public class WallCommandTests
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
		public void DisplayUserWallHasMessagesAndFollowersMessagesTest()
		{
			// act
			string input = "Charlie";
			var user = new User { UserId = Guid.NewGuid(), Username = input };
			var userMessages = new List<Message>
			{
				new Message
				{
					Description = "I’m in New York today! Anyone wants to have a coffee?",
					PostedDt = DateTime.UtcNow.AddSeconds(-15),
					UserId = user.UserId
				}
			};

			var child = new User { UserId = Guid.NewGuid(), Username = "Bob" };
			var followers = new List<User> { child };
			var followersMessages = new List<Message> 
			{ 
				new Message
				{
					Description = "Good game though.", 
					PostedDt = DateTime.UtcNow.AddMinutes(-1),
					UserId = child.UserId
				},
				new Message
				{
					Description = "Damn! We lost!", 
					PostedDt = DateTime.UtcNow.AddMinutes(-2),
					UserId = child.UserId
				}
			};

			var output = new StringBuilder();
			output.AppendLine("Charlie - I’m in New York today! Anyone wants to have a coffee? (15 second(s) ago)");
			output.AppendLine("Bob - Good game though. (1 minute(s) ago)");
			output.AppendLine("Bob - Damn! We lost! (2 minute(s) ago)");

			_userManagerMock.Setup(x => x.GetUserByUsername("Charlie", true)).Returns(user);
			_messageManagerMock.Setup(x => x.GetMessages(user)).Returns(userMessages);
			_userManagerMock.Setup(x => x.GetFollowers(user)).Returns(followers);
			_messageManagerMock.Setup(x => x.GetMessages(child)).Returns(followersMessages);
			_userManagerMock.Setup(x => x.GetUserByUserId(user.UserId, true)).Returns(user);
			_userManagerMock.Setup(x => x.GetUserByUserId(child.UserId, true)).Returns(child);

			var wall = new Wall(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = wall.Perform(input);

			// assert
			Assert.AreEqual(actual, output.ToString());
			_userManagerMock.VerifyAll();
			_messageManagerMock.VerifyAll();
		}

		[Test]
		public void DisplayUserWallHasNoMessagesAndNoFollowersTest()
		{
			// act
			string input = "Alice";
		    var user = new User { UserId = Guid.NewGuid(), Username = input };
			var userMessages = new List<Message>();

			_userManagerMock.Setup(x => x.GetUserByUsername("Alice", true)).Returns(user);
			_messageManagerMock.Setup(x => x.GetMessages(user)).Returns(userMessages);

			var wall = new Wall(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = wall.Perform(input);

			// assert
			Assert.AreEqual(actual, string.Empty);
		}

		[Test]
		public void DisplayUserWallHasNoMessagesAndHasFollowersWithMessagesTest()
		{
			// act
			string input = "Alice";
			var user = new User { UserId = Guid.NewGuid(), Username = input };
			var child = new User { UserId = Guid.NewGuid(), Username = "Bob" };
			var userMessages = new List<Message>();

			var followers = new List<User> {child};
			var followersMessages = new List<Message> { new Message
			{
				Description = "Good game though.", 
				PostedDt = DateTime.UtcNow.AddMinutes(-1),
				UserId = child.UserId
			} };

			var output = new StringBuilder();
			output.AppendLine("Bob - Good game though. (1 minute(s) ago)");

			_userManagerMock.Setup(x => x.GetUserByUsername("Alice", true)).Returns(user);
			_messageManagerMock.Setup(x => x.GetMessages(user)).Returns(userMessages);
			_userManagerMock.Setup(x => x.GetFollowers(user)).Returns(followers);
			_messageManagerMock.Setup(x => x.GetMessages(child)).Returns(followersMessages);
			_userManagerMock.Setup(x => x.GetUserByUserId(child.UserId, true)).Returns(child);

			var wall = new Wall(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = wall.Perform(input);

			// assert
			Assert.AreEqual(actual, output.ToString());
			_userManagerMock.VerifyAll();
			_messageManagerMock.VerifyAll();
		}

		[Test]
		public void DisplayInvalidUserWallTest()
		{
			// act
			string input = "Alice";

			_userManagerMock.Setup(x => x.GetUserByUsername("Alice", true)).Throws(new NullReferenceException("Alice does not exists!"));

			var wall = new Wall(_userManagerMock.Object, _messageManagerMock.Object);

			// actual
			var actual = wall.Perform(input);

			// assert
			Assert.AreEqual(actual, "Alice does not exists!");
		}
	}
}