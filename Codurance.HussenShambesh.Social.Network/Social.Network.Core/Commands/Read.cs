namespace Social.Network.Core.Commands
{
	using System;
	using System.Text;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to read users posts
	/// </summary>
	public class Read : ICommandExecute
	{
		#region local members

		private readonly IUserManager _userManager;
		private readonly IMessageManager _messageManager;

		#endregion

		#region constructors

		public Read(IUserManager userManager, IMessageManager messageManager)
		{
			_userManager = userManager;
			_messageManager = messageManager;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Peform action to read user posts 
		/// </summary>
		/// <param name="input">user input</param>
		/// <returns>result output</returns>
		public string Perform(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return "Cannot Read, User input is empty!";
			}

			StringBuilder output = new StringBuilder();

			try
			{
				string username = input.Trim();

				User parentUser = this._userManager.GetUserByUsername(username, true);

				foreach (Message message in this._messageManager.GetMessages(parentUser))
				{
					output.AppendLine(message.Description + " (" + Time.GetTimeDiff(message.PostedDt) + ")");
				}

				return output.ToString();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		#endregion
	}
}
