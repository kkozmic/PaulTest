using System;
using Autofac;

namespace PaulBenchmark
{
	public class Autofac_delegates : IBenchmark
	{
		private readonly IContainer container;

		public Autofac_delegates()
		{
			var builder = new ContainerBuilder();
			builder.Register(c => new Game()).SingleInstance();
			builder.Register(c => new Player(c.Resolve<Game>(), c.Resolve<Gun>()));
			builder.Register(c => new Gun(c.Resolve<Game>(), c.Resolve<Func<Bullet>>()));
			builder.Register(c => new Bullet(c.Resolve<Game>()));

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