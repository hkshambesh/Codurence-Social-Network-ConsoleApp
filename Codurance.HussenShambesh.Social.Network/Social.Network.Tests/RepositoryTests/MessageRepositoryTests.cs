namespace Social.Network.Tests.RepositoryTests
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using Models;
	using Repository;
	using Repository.Interfaces;

	[TestFixture]
	public class MessageRepositoryTests
	{
		private IMessageRepository _messageRepository;

		[TestFixtureSetUp]
		public void Setup()
		{
			_messageRepository = new MessageRepository();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_messageRepository.Dispose();
		}

		[Test]
		public void AddUserMessageTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "Alice"
			}; 

			var message = new Message
			{
				UserId = user.UserId,
				Description = "I love the weather today",
				MessageId = Guid.NewGuid(),
				PostedDt = DateTime.UtcNow
			};

			_messageRepository.Add(message);

			// actual
			var actual = _messageRepository.Get(message.UserId);

			// assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.UserId, user.UserId);
			Assert.AreEqual(actual.Description, message.Description);
		}

		[Test]
		[ExpectedException]
		public void AddDubplicateUserMessageTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "Alice"
			};

			var message = new Message
			{
				UserId = user.UserId,
				Description = "I love the weather today",
				MessageId = Guid.NewGuid(),
				PostedDt = DateTime.UtcNow
			};

			_messageRepository.Add(message);
			_messageRepository.Add(message);

			// actual
			var actual = _messageRepository.Get();

			// assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.MessageId, message.MessageId);
		}

		[Test]
		public void AddUserMessageChildrenTest()
		{
			// act
			var user = new User
			{
				UserId = Guid.NewGuid(),
				Username = "Bob"
			};

			var userMessage = new Message
			{
				UserId = user.UserId,
			};

			var message1 = new Message
			{
				UserId = user.UserId,
				Description = "Good	game though.",
				MessageId = Guid.NewGuid(),
				PostedDt = DateTime.UtcNow
			};

			var message2 = new Message
			{
				UserId = user.UserId,
				Description = "Damn! We lost!",
				MessageId = Guid.NewGuid(),
				PostedDt = DateTime.UtcNow
			};

			_messageRepository.Add(userMessage);
			_messageRepository.AddChild(message1);
			_messageRepository.AddChild(message2);

			// actual
			var actual = _messageRepository.GetAll(user).ToList();

			// assert
			Assert.AreEqual(actual.First(x => x.MessageId.Equals(message1.MessageId)).MessageId, message1.MessageId);
			Assert.AreEqual(actual.First(x => x.MessageId.Equals(message2.MessageId)).MessageId, message2.MessageId);
			Assert.AreEqual(actual.Count(), 2);
		}

	}
}
