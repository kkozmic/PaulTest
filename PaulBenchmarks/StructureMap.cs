using PaulBenchmark.PaulModel;
using StructureMap;

namespace PaulBenchmark.PaulBenchmarks
{
	public class StructureMap : IBenchmark
	{
		private readonly Container container;

		public StructureMap()
		{
			container = new Container(e =>
			                          	{
			                          		e.ForSingletonOf<Game>().Use<Game>();
			                          		e.For<Player>().Use<Player>();
			                          		e.For<Gun>().Use<Gun>();
			                          		e.For<Bullet>().Use<Bullet>();
			                          	});
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