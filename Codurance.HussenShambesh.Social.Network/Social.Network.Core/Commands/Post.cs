namespace Social.Network.Core.Commands
{
	using System;
	using System.Text.RegularExpressions;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to post messages
	/// </summary>
	public class Post : ICommandExecute
	{
		#region local members

		private readonly IUserManager _userManager;
		private readonly IMessageManager _messageManager;

		#endregion

		#region constructors

		public Post(IUserManager userManager, IMessageManager messageManager)
		{
			_userManager = userManager;
			_messageManager = messageManager;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Peform Post user action
		/// </summary>
		/// <param name="input">user input</param>
		/// <returns>result output</returns>
		public string Perform(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return "Cannot Post, User input is empty!";
			}

			try
			{
				string[] inputStrings = Regex.Split(input, "->");

				string username = inputStrings[0].Trim();
				string postMessage = inputStrings[1].Trim();

				User user = this._userManager.GetUserByUsername(username, false) ?? this._userManager.AddUser(username);

				this._messageManager.AddMessage(user.UserId, postMessage);
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			return string.Empty;
		}

		#endregion
	}
}
