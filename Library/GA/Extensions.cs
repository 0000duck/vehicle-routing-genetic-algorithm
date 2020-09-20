using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GaVrp.Library.GA
{
	public static class Extensions
	{
		/// <summary>
		/// code from :
		/// https://stackoverflow.com/questions/273313/randomize-a-listt
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Shuffle<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}

			//for (var i = 0; i < list.Count; i++)
			//	list.Swap(i, StaticRandom.Instance.Next(i, list.Count));
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}

		/// <summary>
		/// https://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<int> GetRandomNumbers(int count, int maxValue)
		{
			HashSet<int> randomNumbers = new HashSet<int>();

			for (int i = 0; i < count; i++)
				while (!randomNumbers.Add(StaticRandom.Instance.Next(maxValue))) ;

			return randomNumbers;
		}

		public static IList<T> GetRandomList<T>(this IList<T> list, int count)
		{
			var randomNumbers = GetRandomNumbers(count, list.Count - 1);
			var newList = new List<T>();

			foreach (var randomNumber in randomNumbers)
			{
				newList.Add(list[randomNumber]);
			}

			return newList;
		}
	}
}
