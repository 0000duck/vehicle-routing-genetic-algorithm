using System;
using System.Collections.Generic;
using System.Linq;
using GaVrp.Entities.Ga;
using GaVrp.Entities.Map;

namespace GaVrp.Library.GA
{
	public class Crossovers
	{
		private Chromosome parent1;
		private Chromosome parent2;

		private float crossoverRate;

		private int genesCount = 0;
		private int maxIndex = 0;
		private bool[] shuffledMask;
		private int chance = 0;
		public bool[] ShuffleMask
		{
			get
			{
				shuffledMask.Shuffle();
				return shuffledMask;
			}
		}
		public Crossovers(List<Chromosome> parentsChromosomes)
		{
			parent1 = parentsChromosomes[0];
			parent2 = parentsChromosomes[1];
			crossoverRate = GaParameters.CrossOverRate * 100;
			genesCount = parentsChromosomes[0].Customers.Count;
			maxIndex = genesCount - 1;
			chance = StaticRandom.Instance.Next(1, 101);

			shuffledMask = new bool[genesCount];
			for (var i = 0; i < genesCount; i++)
			{
				if (StaticRandom.Instance.NextDouble() <= 0.5)
				{
					shuffledMask[i] = true;
				}
			}
		}

		public List<Chromosome> PerformUox()
		{
			if (chance > crossoverRate)
			{
				return new List<Chromosome>()
				{
					parent1,
					parent2
				};
			}

			var mask = ShuffleMask;

			var child1 = new Customer[genesCount];
			var child1Ids = new HashSet<int>();
			var child2 = new Customer[genesCount];
			var child2Ids = new HashSet<int>();

			for (var i = 0; i < genesCount; i++)
			{
				if (mask[i])
				{
					child1[i] = parent1.Customers[i];
					child1Ids.Add(child1[i].Id);

					child2[i] = parent2.Customers[i];
					child2Ids.Add(child2[i].Id);
				}
			}

			for (var i = 0; i < genesCount; i++)
			{
				if (child1[i] == null)
				{
					child1[i] = parent2.Customers.First(x => !child1Ids.Contains(x.Id));
					child1Ids.Add(child1[i].Id);
				}

				if (child2[i] == null)
				{
					child2[i] = parent1.Customers.First(x => !child2Ids.Contains(x.Id));
					child2Ids.Add(child2[i].Id);
				}
			}

			return new List<Chromosome>()
			{
				new Chromosome(child1.ToList(),parent1.Deposit),
				new Chromosome(child2.ToList(),parent1.Deposit),
			};

			//var child1 = parent1.Copy();
			//var child2 = parent2.Copy();
		}


		public List<Chromosome> PerformPmx()
		{
			if (chance > crossoverRate)
			{
				return new List<Chromosome>()
				{
					parent1,
					parent2
				};
			}

			var cutPoint1 = 0;
			var cutPoint2 = 0;

			while (cutPoint1 == cutPoint2)
			{
				cutPoint1 = StaticRandom.Instance.Next(0, maxIndex);
				cutPoint2 = StaticRandom.Instance.Next(0, maxIndex);
			}

			var child1 = new Customer[genesCount];
			var child1Ids = new HashSet<int>();
			var child2 = new Customer[genesCount];
			var child2Ids = new HashSet<int>();

			for (var i = 0; i < genesCount; i++)
			{
				if (!InTheCutPoints(i, cutPoint1, cutPoint2))
					continue;

				//swap
				child1[i] = parent2.Customers[i];
				child1Ids.Add(child1[i].Id);

				child2[i] = parent1.Customers[i];
				child2Ids.Add(child2[i].Id);
			}

			//Fill with no conflict
			for (var i = 0; i < genesCount; i++)
			{
				if (InTheCutPoints(i, cutPoint1, cutPoint2))
					continue;

				if (!child1Ids.Contains(parent1.Customers[i].Id))
				{
					child1[i] = parent1.Customers[i];
					child1Ids.Add(child1[i].Id);
				}

				if (!child2Ids.Contains(parent2.Customers[i].Id))
				{
					child2[i] = parent1.Customers[i];
					child2Ids.Add(child2[i].Id);
				}
			}

			//Fill the remaining
			for (var i = 0; i < genesCount; i++)
			{
				if (InTheCutPoints(i, cutPoint1, cutPoint2))
					continue;

				if (child1[i] == null)
				{
					child1[i] = parent1.Customers.First(x => !child1Ids.Contains(x.Id));
					child1Ids.Add(child1[i].Id);
				}

				if (child2[i] == null)
				{
					child2[i] = parent2.Customers.First(x => !child2Ids.Contains(x.Id));
					child2Ids.Add(child2[i].Id);
				}
			}

			return new List<Chromosome>()
			{
				new Chromosome(child1.ToList(),parent1.Deposit),
				new Chromosome(child2.ToList(),parent1.Deposit),
			};

		}

		private static bool InTheCutPoints(int i, int cutPoint1, int cutPoint2)
		{
			if (i >= cutPoint1 && i <= cutPoint2)
			{
				return true;
			}
			return false;
		}

		#region Cyclic
		/// Got some parts from:
		/// https://github.com/giacomelli/GeneticSharp/blob/master/src/GeneticSharp.Domain/Crossovers/CycleCrossover.cs

		public List<Chromosome> PerformCx()
		{
			if (chance > crossoverRate)
			{
				return new List<Chromosome>()
				{
					parent1,
					parent2
				};
			}

			var child1 = new Customer[genesCount];
			var child2 = new Customer[genesCount];

			var cycles = new List<List<int>>();

			for (int i = 0; i < parent1.Customers.Count; i++)
			{
				if (!cycles.SelectMany(p => p).Contains(i))
				{
					var cycleList = new List<int>();
					CreateCycle(parent1.Customers, parent2.Customers, i, cycleList);
					cycles.Add(cycleList);
				}
			}

			for (int cycleIndex = 0; cycleIndex < cycles.Count; cycleIndex++)
			{
				var cycle = cycles[cycleIndex];

				if (cycleIndex % 2 == 0)
				{

					for (int i = 0; i < genesCount; i++)
					{
						if (cycle.Contains(i))
						{
							child1[i] = parent1.Customers[i];
							child2[i] = parent2.Customers[i];

						}
					}
				}
				else
				{
					for (int i = 0; i < genesCount; i++)
					{
						if (cycle.Contains(i))
						{
							child1[i] = parent2.Customers[i];
							child2[i] = parent1.Customers[i];
						}
					}
				}
			}

			return new List<Chromosome>()
			{
				new Chromosome(child1.ToList(),parent1.Deposit),
				new Chromosome(child2.ToList(),parent1.Deposit),
			};

		}

		private void CreateCycle(List<Customer> parent1Customers, List<Customer> parent2Customers, int geneIndex, List<int> cycle)
		{
			while (true)
			{
				if (!cycle.Contains(geneIndex))
				{
					var parent2Gene = parent2Customers[geneIndex];
					cycle.Add(geneIndex);
					var newGeneIndex = parent1Customers.Select((g, i) => new {g.Id, Index = i}).First(g => g.Id.Equals(parent2Gene.Id));

					if (geneIndex != newGeneIndex.Index)
					{
						geneIndex = newGeneIndex.Index;
						continue;
					}
				}
				break;
			}
		}

		#endregion


		public List<Chromosome> PerformBcrc()
		{
			throw new NotImplementedException();
		}
	}
}
