namespace Social.Network.Core.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to display Wall of user
	/// </summary>
	public class Wall : ICommandExecute
	{
		#region local members

		private readonly IUserManager _userManager;
		private readonly IMessageManager _messageManager;

		#endregion

		#region constructors

		public Wall(IUserManager userManager, IMessageManager messageManager)
		{
			_userManager = userManager;
			_messageManager = messageManager;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Peform action to display user wall 
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
				string[] inputStrings = input.Split(' ');

				string parentUsername = inputStrings[0].Trim();

				User parentUser = this._userManager.GetUserByUsername(parentUsername, true);

				// get parent user messages
				List<Message> userWall = this._messageManager.GetMessages(parentUser).ToList();

				// get followers messages and add them to the wall list
				userWall.AddRange(this._userManager.GetFollowers(parentUser).SelectMany(user => this._messageManager.GetMessages(user)));

				// display wall messages
				foreach (Message message in userWall.OrderByDescending(x => x.PostedDt))
				{
					string username = this._userManager.GetUserByUserId(message.UserId, true).Username;

					output.AppendLine(username + " - " + message.Description + " (" + Time.GetTimeDiff(message.PostedDt) + ")");
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
