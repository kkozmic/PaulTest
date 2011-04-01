using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.Core.Internal;

namespace PaulBenchmark
{
	public abstract class BenchmarkEngine : IDisposable
	{
		protected Regex filter;
		protected int iterations;
		protected TextWriter output = TextWriter.Null;

		protected abstract void Go();

		public void Run()
		{
			Warmup();
			Go();
		}

		public virtual void Dispose()
		{
			if (output != null)
			{
				output.WriteLine();
				output.Dispose();
				output = null;
			}
		}

		protected void Run(IBenchmark benchmark, Func<IBenchmark, long> function)
		{
			if (ShouldRun(filter, benchmark))
			{
				output.Write(function(benchmark) + "\t");
			}
			else
			{
				output.Write("0\t");
			}
		}

		private void Warmup()
		{
			foreach (var benchmark in GetTestSubjects())
			{
				benchmark.Run();
			}
		}

		protected static IBenchmark[] GetTestSubjects()
		{
			return Assembly.GetExecutingAssembly()
				.GetExportedTypes()
				.Where(t => t.IsClass)
				.Where(t => t.IsAbstract == false)
				.Where(t => t.Is<IBenchmark>())
				.Select(Activator.CreateInstance)
				.Cast<IBenchmark>()
				.ToArray();
		}

		private static bool ShouldRun(Regex filter, IBenchmark benchmark)
		{
			if (filter == null) return true;
			var name = benchmark.GetType().Name;
			var shouldRun = filter.IsMatch(name);
			return shouldRun;
		}
	}
}