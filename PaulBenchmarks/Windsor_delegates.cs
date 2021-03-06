﻿using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using PaulBenchmark.PaulModel;

namespace PaulBenchmark.PaulBenchmarks
{
	public class Windsor_delegates : IBenchmark
	{
		private readonly IWindsorContainer container;

		public Windsor_delegates()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.Register(Component.For<Game>().UsingFactoryMethod(() => new Game()),
				          Component.For<Player>().UsingFactoryMethod(k => new Player(k.Resolve<Game>(), k.Resolve<Gun>())).LifeStyle
				          	.Transient,
				          Component.For<Gun>().UsingFactoryMethod(k => new Gun(k.Resolve<Game>(), k.Resolve<Func<Bullet>>())).
				          	LifeStyle.Transient,
				          Component.For<Bullet>().UsingFactoryMethod(k => new Bullet(k.Resolve<Game>())).LifeStyle.Transient,
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