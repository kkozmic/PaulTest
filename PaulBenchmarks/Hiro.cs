using System;
using Hiro;
using Hiro.Containers;
using PaulBenchmark.PaulModel;

namespace PaulBenchmark.PaulBenchmarks
{
	public class Hiro : IBenchmark
	{
		private readonly IMicroContainer container;

		public Hiro()
		{
			var map = new DependencyMap();
			map.AddSingletonService<Game, Game>();
			map.AddService<Player, Player>();
			map.AddService<Gun, Gun>();
			map.AddService<Bullet, Bullet>();
			map.AddService<Func<Bullet>>(k => () => k.GetInstance<Bullet>());
			container = map.CreateContainer();
		}


		public Player ResolvePlayer()
		{
			return container.GetInstance<Player>();
		}

		public void Run()
		{
			var player = ResolvePlayer();
			player.Shoot();
		}
	}
}