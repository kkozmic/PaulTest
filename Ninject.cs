using Ninject;

namespace PaulBenchmark
{
	public class Ninject : IBenchmark
	{
		private readonly StandardKernel kernel;

		public Ninject()
		{
			kernel = new StandardKernel(new RegisterNinjectModule());
		}

		public Player ResolvePlayer()
		{
			return kernel.TryGet<Player>();
		}

		public void Run()
		{
			var player = ResolvePlayer();
			player.Shoot();
		}
	}
}