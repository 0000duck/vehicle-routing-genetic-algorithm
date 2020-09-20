using System;

namespace GaVrp.Library
{
	public static class Logger
	{
		public static void Log(string txt)
		{
			Console.WriteLine(txt);
		}

		public static void AddSpace()
		{
			Logger.Log("*******************");
		}
	}
}
