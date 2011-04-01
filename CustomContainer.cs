namespace PaulBenchmark
{
	public class CustomContainer : IBenchmark
	{
		private readonly Game game = new Game();

		public Bullet ResolveBullet()
		{
			return new Bullet(game);
		}

		public Game ResolveGame()
		{
			return game;
		}

		public Gun ResolveGun()
		{
			return new Gun(game, ResolveBullet);
		}

		public Player ResolvePlayer()
		{
			return new Player(game, ResolveGun());
		}

		public void Run()
		{
			var player = ResolvePlayer();
			player.Shoot();
		}
	}
}