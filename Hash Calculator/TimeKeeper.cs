using System;
using System.Diagnostics;

namespace Hash_Calculator
{
	class TimeKeeper
	{
		public TimeSpan Measure(Action action)
		{
			var watch = new Stopwatch();

			watch.Start();
			action();

			return watch.Elapsed;
		}
	}
}
