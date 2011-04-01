using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using PaulBenchmark.TurbineModel;

namespace PaulBenchmark.TurbineBenchmarks
{
	public class Windsor_Turbine : IBenchmark
	{
		private readonly WindsorContainer container;

		public Windsor_Turbine()
		{
			container = new WindsorContainer();
			container.Register(
				Component.For<RootComponent>().LifeStyle.Transient,
				AllTypes.FromThisAssembly()
					.Where(Component.IsInSameNamespaceAs<RootComponent>())
					.WithService.AllInterfaces()
					.Configure(c => c.LifeStyle.Transient));
		}

		public void Run()
		{
			var root = container.Resolve<RootComponent>();
			container.Release(root);
		}
	}
}