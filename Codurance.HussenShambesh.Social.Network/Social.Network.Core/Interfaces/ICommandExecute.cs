namespace Social.Network.Core.Interfaces
{
	public interface ICommandExecute
	{
		/// <summary>
		/// Peform user action by Command type
		/// </summary>
		/// <param name="input">user input</param>
		/// <returns>result output</returns>
		string Perform(string input);
	}
}