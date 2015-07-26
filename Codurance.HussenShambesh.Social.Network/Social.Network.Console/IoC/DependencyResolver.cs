namespace Social.Network.Console.IoC
{
	using System;
	using System.Reflection;
	using Ninject;

	/// <summary>
	/// singleton class to resolve ninject modules 
	/// </summary>
	public static class DependencyResolver
	{
		private static readonly Lazy<IKernel> Instance = new Lazy<IKernel>(() =>
		{
			IKernel kernel = new StandardKernel();
			kernel.Load(Assembly.GetExecutingAssembly());

			return kernel;
		});

		public static IKernel Kernel
		{
			get { return Instance.Value; }
		}
	}
}