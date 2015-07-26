namespace Social.Network.Models
{
	using System;

	public class Message
	{
		public Guid MessageId { get; set; }
		public string Description { get; set; }
		public DateTime PostedDt { get; set; }

		public Guid UserId { get; set; }
	}
}