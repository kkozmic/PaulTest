using System;
using Microsoft.Practices.Unity;

namespace PaulBenchmark
{
	public class Unity : IBenchmark
	{
		private readonly UnityContainer container;

		public Unity()
		{
			container = new UnityContainer();
			container.RegisterType<Game>(new ContainerControlledLifetimeManager());
			container.RegisterType<Player>(new TransientLifetimeManager());
			container.RegisterType<Gun>(new TransientLifetimeManager());
			container.RegisterType<Bullet>(new TransientLifetimeManager());
			container.RegisterInstance(new Func<Bullet>(() => container.Resolve<Bullet>()));
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