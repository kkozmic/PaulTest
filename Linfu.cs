using System;
using LinFu.IoC;
using LinFu.IoC.Configuration;

namespace PaulBenchmark
{
	public class Linfu : IBenchmark
	{
		private readonly ServiceContainer container;

		public Linfu()
		{
			container = new ServiceContainer();
			container.AddService(typeof (Game), LifecycleType.Singleton);
			container.AddService(typeof (Player), LifecycleType.OncePerRequest);
			container.AddService(typeof (Gun), LifecycleType.OncePerRequest);
			container.AddService(typeof (Bullet), LifecycleType.OncePerRequest);
			container.AddService<Func<Bullet>>(r => () => r.Container.GetService<Bullet>(), LifecycleType.OncePerRequest);
		}

		public Player ResolvePlayer()
		{
			return container.GetService<Player>();
		}

		public void Run()
		{
			var player = ResolvePlayer();
			player.Shoot();
		}
	}
}