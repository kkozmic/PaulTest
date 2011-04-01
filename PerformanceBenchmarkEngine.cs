using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PaulBenchmark
{
	public class PerformanceBenchmarkEngine : BenchmarkEngine
	{
		private static readonly long oneSecondTicks = TimeSpan.FromSeconds(1).Ticks;
		private readonly Mode mode;
		private readonly Results results;

		public PerformanceBenchmarkEngine(Mode mode, int iterations, Regex filter, string outputFile, Results results)
		{
			this.mode = mode;
			this.iterations = iterations;
			this.filter = filter;
			this.results = results;
			if (outputFile != null)
			{
				output = new StreamWriter(outputFile, true, Encoding.UTF8);
			}
		}


		protected override void Go()
		{
			output.Write(mode + "\t" + iterations + "\t");
			Console.WriteLine("Running {0} times in {1} mode", iterations, mode);
			var tests = GetTestSubjects();
			switch (mode)
			{
				case Mode.Task_based:
					Console.WriteLine("...on multiple threads (using TPL Tasks)");
					foreach (var paulTest in tests)
					{
						Run(paulTest, RunTaskBased);
					}
					break;
				case Mode.Thread_based:
					Console.WriteLine("...on multiple threads (using {0} Threads)", Environment.ProcessorCount);
					foreach (var paulTest in tests)
					{
						Run(paulTest, RunThreadBased);
					}
					break;
				default:
					Console.WriteLine("...on a single thread");
					foreach (var paulTest in tests)
					{
						Run(paulTest, RunSingleThread);
					}
					break;
			}
		}

		private void PrintResult(IBenchmark test, Stopwatch stopwatch)
		{
			if (results == Results.Total_time)
			{
				Console.WriteLine(" Hey {0,-20} - you did it in {1,10}", test.GetType().Name, stopwatch.Elapsed);
			}
			else
			{
				Console.WriteLine(" Hey {0,-20} - you did {1,10} operations per second", test.GetType().Name,
				                  (iterations*oneSecondTicks)/stopwatch.Elapsed.Ticks);
			}
		}

		private long RunSingleThread(IBenchmark test)
		{
			var stopwatch = Stopwatch.StartNew();
			for (var i = 0; i < iterations; i++)
			{
				test.Run();
			}
			stopwatch.Stop();
			PrintResult(test, stopwatch);
			var disposable = test as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return stopwatch.ElapsedMilliseconds;
		}

		private long RunTaskBased(IBenchmark test)
		{
			var stopwatch = Stopwatch.StartNew();
			var tasks = new Task[iterations];
			for (var i = 0; i < tasks.Length; i++)
			{
				tasks[i] = Task.Factory.StartNew(test.Run);
			}
			Task.WaitAll(tasks);
			stopwatch.Stop();
			PrintResult(test, stopwatch);
			var disposable = test as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return stopwatch.ElapsedMilliseconds;
		}

		private long RunThreadBased(IBenchmark test)
		{
			var stopwatch = Stopwatch.StartNew();
			var threads = new Thread[Environment.ProcessorCount];
			var interval = iterations/threads.Length;
			for (var i = 0; i < threads.Length; i++)
			{
				var thread = new Thread(() =>
				                        	{
				                        		for (var j = 0; j < interval; j++)
				                        		{
				                        			test.Run();
				                        		}
				                        	});
				threads[i] = thread;
				thread.Start();
			}
			for (var i = 0; i < threads.Length; i++)
			{
				threads[i].Join();
			}
			stopwatch.Stop();
			PrintResult(test, stopwatch);
			var disposable = test as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return stopwatch.ElapsedMilliseconds;
		}
	}
}