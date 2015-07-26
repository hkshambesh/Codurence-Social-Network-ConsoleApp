namespace Social.Network.Core.Interfaces
{
	public interface ICommandFactory
	{
		/// <summary>
		/// Factory method to get the execute the comman based on its type
		/// </summary>
		/// <param name="command">command type</param>
		/// <returns>Injecting ICommandExecute interface</returns>
		ICommandExecute GetCommandExecuteHandler(string command);
	}
}