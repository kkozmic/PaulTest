using Funq;

namespace PaulBenchmark
{
	public class Funq : IBenchmark
	{
		private readonly Container container;

		public Funq()
		{
			container = new Container();
			container.Register(c => new Game()).ReusedWithin(ReuseScope.Container);
			container.Register(c => new Player(c.Resolve<Game>(), c.Resolve<Gun>())).ReusedWithin(ReuseScope.None);
			container.Register(c => new Gun(c.Resolve<Game>(), c.LazyResolve<Bullet>())).ReusedWithin(ReuseScope.None);
			container.Register(c => new Bullet(c.Resolve<Game>())).ReusedWithin(ReuseScope.None);
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