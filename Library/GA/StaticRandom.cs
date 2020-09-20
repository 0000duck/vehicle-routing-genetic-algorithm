using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GaVrp.Library.GA
{
	/// <summary>
	/// Code from:
	/// https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number?noredirect=1&lq=1
	/// Used this clean code due to the Random problem if created every time in loop
	/// </summary>
	public static class StaticRandom
	{
		private static int seed;

		private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
			(() => new Random(Interlocked.Increment(ref seed)));

		static StaticRandom()
		{
			seed = Environment.TickCount;
		}

		public static Random Instance { get { return threadLocal.Value; } }
	}
}
