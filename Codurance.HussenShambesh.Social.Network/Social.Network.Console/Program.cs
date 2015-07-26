namespace Social.Network.Console
{
	using Ninject;
	using IoC;
	using Core.Interfaces;

	/// <summary>
	/// class for the main startup program
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{
			IProgramManager programManager = DependencyResolver.Kernel.Get<IProgramManager>();

			programManager.Start();
		}
	}
}
