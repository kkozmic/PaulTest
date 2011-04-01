using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace PaulBenchmark
{
	public class Windsor : IBenchmark
	{
		private readonly IWindsorContainer container;

		public Windsor()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.Register(Component.For<Game>(),
				          Component.For<Player>().LifeStyle.Transient,
				          Component.For<Gun>().LifeStyle.Transient,
				          Component.For<Bullet>().LifeStyle.Transient,
				          Component.For<Func<Bullet>>().AsFactory());
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