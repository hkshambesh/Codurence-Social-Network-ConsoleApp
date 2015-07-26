using System;
using Moq;
using Social.Network.Core.Managers;
using Social.Network.Models;
using Social.Network.Repository.Interfaces;

namespace Social.Network.Tests.CoreTests
{
	using NUnit.Framework;

	[TestFixture]
	public class MessageManagerTests
	{
		private Mock<IMessageRepository> _messageRepositoryMock;

		[TestFixtureSetUp]
		public void Setup()
		{
			_messageRepositoryMock = new Mock<IMessageRepository>();
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			_messageRepositoryMock = null;
		}

		[Test]
		public void AddMessageForExistingUserTest()
		{
			// act
			_messageRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new Message());
			_messageRepositoryMock.Setup(x => x.AddChild(It.IsAny<Message>()));

			var messageManager = new MessageManager(_messageRepositoryMock.Object);

			// actual
			messageManager.AddMessage(Guid.NewGuid(), "test message");

			// assert
			_messageRepositoryMock.Verify(x => x.Add(It.IsAny<Message>()), Times.Never);
		}

		[Test]
		public void AddMessageForNewUserTest()
		{
			// act
			_messageRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Message) null);
			_messageRepositoryMock.Setup(x => x.AddChild(It.IsAny<Message>()));

			var messageManager = new MessageManager(_messageRepositoryMock.Object);

			// actual
			messageManager.AddMessage(Guid.NewGuid(), "test message");

			// assert
			_messageRepositoryMock.Verify(x=>x.Add(It.IsAny<Message>()), Times.Once);
		}	
	}
}