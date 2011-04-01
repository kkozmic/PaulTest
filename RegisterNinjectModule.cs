using System;
using Ninject;
using Ninject.Modules;

namespace PaulBenchmark
{
	public class RegisterNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind<Game>().To<Game>().InSingletonScope();
			Bind<Player>().To<Player>().InTransientScope();
			Bind<Gun>().To<Gun>().InTransientScope();
			Bind<Bullet>().To<Bullet>().InTransientScope();
			Bind<Func<Bullet>>().ToMethod(c => () => c.Kernel.TryGet<Bullet>());
		}
	}
}