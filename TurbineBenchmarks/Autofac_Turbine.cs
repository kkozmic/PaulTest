using Autofac;
using PaulBenchmark.TurbineModel;

namespace PaulBenchmark.TurbineBenchmarks
{
	public class Autofac_Turbine : IBenchmark
	{
		private IContainer container;

		public Autofac_Turbine()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<RootComponent>();
			builder.RegisterAssemblyTypes(GetType().Assembly)
				.Where(t => t.IsClass && t.Namespace == typeof (RootComponent).Namespace)
				.AsImplementedInterfaces();
			container = builder.Build();
		}

		public void Run()
		{
			container.Resolve<RootComponent>();
		}
	}
}