namespace Social.Network.Core.Commands
{
	using System;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to follow users
	/// </summary>
	public class Follow : ICommandExecute
	{
		#region local members

		private readonly IUserManager _userManager;

		#endregion

		#region constructors

		public Follow(IUserManager userManager)
		{
			_userManager = userManager;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Peform action to follow users 
		/// </summary>
		/// <param name="input">user input</param>
		/// <returns>result output</returns>
		public string Perform(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return "Cannot Follow, User input is empty!";
			}

			try
			{
				string[] inputStrings = input.Split(' ');

				string username = inputStrings[0].Trim();
				string userToFollow = inputStrings[2].Trim();

				User parentUser = this._userManager.GetUserByUsername(username, true);

				User followedUser = this._userManager.GetUserByUsername(userToFollow, true);

				this._userManager.CheckFollowerExists(parentUser, followedUser);

				this._userManager.FollowUser(parentUser, followedUser);
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
