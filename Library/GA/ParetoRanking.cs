using System.Collections.Generic;
using System.Linq;

namespace GaVrp.Library.GA
{
	/// <summary>
	/// Converted the Java code from https://github.com/royerloic/orionlib/blob/bd353017723f2f2071d33bf482da2eee30bec046/OrionLib/src/utils/pareto/ParetoRanking.java
	/// to C# 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ParetoRanking<T>
	{
		private readonly Dictionary<Vector, double> rankings = new Dictionary<Vector, double>();

		private readonly Dictionary<object, Vector> vectors = new Dictionary<object, Vector>();

		public void AddVector(T pObject, params double[] pValues)
		{
			var lVector = new Vector
			{
				mObject = pObject,
				mValues = pValues
			};
			vectors.Add(pObject, lVector);
		}

		public void AddVector(T pObject, List<double> pValues)
		{
			var lVector = new Vector {mObject = pObject};
			var lValues = new double[pValues.Count()];
			for (var i = 0; i < pValues.Count; i++)
				lValues[i] = pValues[i];

			lVector.mValues = lValues;
			vectors.Add(pObject, lVector);
		}

		public int ComputeRanking()
		{
			return ComputeRanking(int.MaxValue);
		}

		public int ComputeRanking(int pMaxLayer)
		{
			var lWorkingSet = new HashSet<Vector>(vectors.Values);
			var lParetoFront = new HashSet<Vector>();
			var lParetoFrontDel = new HashSet<Vector>();
			var lParetoFrontAdd = new HashSet<Vector>();
			double lLayer = 0;
			while (lWorkingSet.Any() && lLayer <= pMaxLayer)
			{
				foreach (var lVector in lWorkingSet)
				{
					lParetoFrontDel.Clear();
					lParetoFrontAdd.Clear();
					foreach (var lVectorFromFront in lParetoFront)
						if (Dominate(lVector.mValues, lVectorFromFront.mValues))
						{
							//  new vector dominates vector in front
							lParetoFrontDel.Add(lVectorFromFront);
						}
						else if (Dominate(lVectorFromFront.mValues, lVector.mValues))
						{
							//  new vector dominated by vector in front, do nothing
						}
						else
						{
							lParetoFrontAdd.Add(lVector);
						}

					if (lParetoFrontDel.Count > 0
					    || !lParetoFront.Any())
						lParetoFrontAdd.Add(lVector);

					lParetoFront.ExceptWith(lParetoFrontDel);
					lParetoFront.UnionWith(lParetoFrontAdd);
				}

				foreach (var lVector in lParetoFront)
					rankings.Add(lVector, lLayer);

				lWorkingSet.ExceptWith(lParetoFront);
				lParetoFront.Clear();
				lLayer++;
			}

			return (int) lLayer;
		}

		public Dictionary<Vector, double> GetRankings()
		{
			return rankings;
		}

		public double GetRanking(T pObject)
		{
			return rankings[vectors[pObject]];
		}

		private static bool Dominate(double[] v1, double[] v2)
		{
			if (Equal(v1, v2))
				return false;

			for (var i = 0; i < v1.Length; i++)
				if (v1[i] < v2[i])
					return false;

			return true;
		}

		private static bool Equal(double[] v1, double[] v2)
		{
			for (var i = 0; i < v1.Length; i++)
			{
				if (v1[i] != v2[i])
				{
					return false;
				}
			}

			return true;
		}

		public class Vector
		{
			public T mObject;

			public double[] mValues;
		}
	}
}