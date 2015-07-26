namespace Social.Network.Console.IoC
{
	using Ninject.Extensions.Factory;
	using Ninject.Modules;
	using Core.Commands;
	using Core.Interfaces;
	using Core.Managers;
	using Models;
	using Repository;
	using Repository.Interfaces;

	/// <summary>
	/// module class for Dependency Injection
	/// </summary>
	public class ProgramModule : NinjectModule
	{
		public override void Load()
		{
			this.Bind<IProgramManager>().To<ProgramManager>();
			this.Bind<IUserManager>().To<UserManager>();
			this.Bind<IMessageManager>().To<MessageManager>();

			// repositories are used in the same thread and disposed at the end of each thread
			this.Bind<IUserRepository>().To<UserRepository>().InThreadScope();
			this.Bind<IMessageRepository>().To<MessageRepository>().InThreadScope();

			// factory pattern by command name type
			this.Bind<ICommandExecute>().To<Post>().Named(CommandType.Post.ToString());
			this.Bind<ICommandExecute>().To<Read>().Named(CommandType.Read.ToString());
			this.Bind<ICommandExecute>().To<Follow>().Named(CommandType.Follow.ToString());
			this.Bind<ICommandExecute>().To<Wall>().Named(CommandType.Wall.ToString());
		
			this.Bind<ICommandFactory>().ToFactory(() => new NameInstanceProvider());
		}
	}
}