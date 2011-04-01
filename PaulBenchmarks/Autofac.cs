using Autofac;
using PaulBenchmark.PaulModel;

namespace PaulBenchmark.PaulBenchmarks
{
	public class Autofac : IBenchmark
	{
		private readonly IContainer container;

		public Autofac()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<Game>().SingleInstance();
			builder.RegisterType<Player>().InstancePerDependency();
			builder.RegisterType<Gun>().InstancePerDependency();
			builder.RegisterType<Bullet>().InstancePerDependency();

			container = builder.Build();
		}

		public Player ResolvePlayer()
		{
			return container.Resolve<Player>();
		}

		public void Run()
		{
			var player = ResolvePlayer();
			player.Shoot();
		}
	}
}