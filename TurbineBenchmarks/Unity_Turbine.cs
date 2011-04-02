using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using PaulBenchmark.TurbineModel;

namespace PaulBenchmark.TurbineBenchmarks
{
	public class Unity_Turbine : IBenchmark
	{
		private static readonly Assembly assembly = typeof (Unity_Turbine).Assembly;
		private readonly UnityContainer container;

		public Unity_Turbine()
		{
			container = new UnityContainer();

			container.RegisterType<RootComponent>(new TransientLifetimeManager());
			foreach (var type in GetPossibleInterfacesToRegister())
			{
				RegisterThisInterfaceWithASingleImplementer(type, container);
			}
		}

		public void Run()
		{
			container.Resolve<RootComponent>();
		}


		private void RegisterThisInterfaceWithASingleImplementer(Type @interface, IUnityContainer locator)
		{
			var implementers = GetImplementers(@interface);
			if (ThereIsOnlyOneImplementerOfThisInterface(implementers))
				RegisterTheTwo(@interface, implementers, locator);
		}

		private void RegisterTheTwo(Type @interface, IEnumerable<Type> implementers, IUnityContainer locator)
		{
			var type = implementers.Single();
			locator.RegisterType(@interface, type, new TransientLifetimeManager());
		}

		private bool ThereIsOnlyOneImplementerOfThisInterface(IEnumerable<Type> implementers)
		{
			return implementers.Count() == 1;
		}

		private IEnumerable<Type> GetImplementers(Type @interface)
		{
			return assembly.GetExportedTypes()
				.Where(t => t.Namespace == typeof (RootComponent).Namespace)
				.Where(x => x.GetInterfaces().Contains(@interface));
		}

		private List<Type> GetPossibleInterfacesToRegister()
		{
			return assembly
				.GetTypes().Where(x => x.IsInterface)
				.ToList();
		}
	}
}