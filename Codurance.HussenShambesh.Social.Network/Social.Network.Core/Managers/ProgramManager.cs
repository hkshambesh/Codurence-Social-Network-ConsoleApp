namespace Social.Network.Core.Managers
{
	using System;
	using System.Linq;
	using Interfaces;
	using Models;

	/// <summary>
	/// class to manage the program execution
	/// </summary>
	public class ProgramManager : IProgramManager
	{
		#region local members

		private ICommandExecute _cmdExecute;
		private CommandType _commandType;
		private readonly ICommandFactory _commandFactory;

		#endregion

		#region constructors

		public ProgramManager(ICommandFactory commandFactory)
		{
			_commandFactory = commandFactory;
		}

		#endregion

		#region public methods

		/// <summary>
		/// method to start the main console application
		/// </summary>
		public void Start()
		{
			Console.WriteLine("Please enter command:");

			while (true)
			{
				string input = Console.ReadLine();

				try
				{
					if (!this.ValidateInput(input))
					{
						continue;
					}

					this._cmdExecute = this._commandFactory.GetCommandExecuteHandler(this._commandType.ToString());

					string output = this._cmdExecute.Perform(input);

					if (!string.IsNullOrWhiteSpace(output))
					{
						Console.WriteLine(output);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		#endregion

		#region private methods

		/// <summary>
		/// method to validate user input and assign correct command
		/// </summary>
		/// <param name="input">user input</param>
		/// <returns>a boolean indicating whether the input is valid</returns>
		private bool ValidateInput(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				Console.WriteLine("Invalid input, try again:");
				return false;
			}

			if (input.Contains("->"))
			{
				_commandType = CommandType.Post;
			}
			else if (input.Contains("wall"))
			{
				_commandType = CommandType.Wall;
			}
			else if (input.Contains("follows"))
			{
				_commandType = CommandType.Follow;
			}
			else if (input.Split(' ').Count() == 1)
			{
				_commandType = CommandType.Read;
			}
			else
			{
				_commandType = CommandType.Unknown;

				Console.WriteLine("Unknown Command, try again!");
				return false;
			}

			return true;
		}

		#endregion
	}
}
