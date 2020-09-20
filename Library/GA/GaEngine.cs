using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GaVrp.Entities;
using GaVrp.Entities.Ga;
using GaVrp.Entities.Map;

namespace GaVrp.Library.GA
{
	public enum CrossoverType
	{
		None,
		Uox,
		Cx,
		Pmx,
		Bcrc
	}

	public enum FitnessMethod
	{
		None,
		WeightedSum,
		Pareto,
	}

	public class GaEngine
	{
		public List<Chromosome> Children { get; set; }
		public List<Chromosome> Parents { get; set; }

		private List<Chromosome> sortedParents
		{
			get
			{
				return ComputeFitnessAndOrder(Parents);
			}
		}


		public void SetParentsAndResetChildren(List<Chromosome> parents)
		{
			Parents = parents;
			Children = new List<Chromosome>();
		}

		private List<Chromosome> ComputeFitnessAndOrder(IList<Chromosome> chromosomes)
		{
			if (GaParameters.FitnessMethod == FitnessMethod.WeightedSum)
			{
				foreach (var chromosome in chromosomes)
				{
					chromosome.Fitness = chromosome.CalculateWeightedSum();
				}
				return chromosomes.OrderBy(x => x.Fitness).ToList();

			}
			else if (GaParameters.FitnessMethod == FitnessMethod.Pareto)
			{
				var paretoRanking = chromosomes.First().ComputeParetoRank(chromosomes);

				for (var i = 0; i < chromosomes.Count; i++)
				{
					chromosomes[i].Rank = chromosomes[i].GetParetoRank(paretoRanking);
					chromosomes[i].Fitness = chromosomes[i].CalculateWeightedSum();

				}
				//since pareto ranking returns a set of best chromosomes, I select the best of rank by weighted sum value
				//to compare the results with wieghted sum function
				return chromosomes.OrderBy(x => x.Rank).ThenBy(x => x.Fitness).ToList(); 
			}

			return new List<Chromosome>(); //throw exception instead
		}

		public List<Chromosome> PerformCrossover(CrossoverType crossoverType, List<Chromosome> parentsChromosomes)
		{
			var crossover = new Crossovers(parentsChromosomes);
			if (crossoverType == CrossoverType.Cx)
			{
				return crossover.PerformCx();
			}

			if (crossoverType == CrossoverType.Uox)
			{
				return crossover.PerformUox();
			}

			if (crossoverType == CrossoverType.Pmx)
			{
				return crossover.PerformPmx();
			}

			if (crossoverType == CrossoverType.Bcrc)
			{
				return crossover.PerformBcrc();
			}

			throw new Exception("Crossover type not supported.");
		}

		public List<Chromosome> InitializePopulation(List<Customer> customers, Deposit deposit)
		{
			var chormosomes = new List<Chromosome>();
			for (int i = 0; i < GaParameters.PopulationSize; i++)
			{
				var randomCustomers = new List<Customer>();
				randomCustomers.AddRange(customers);
				randomCustomers.Shuffle();
				var chromosome = new Chromosome(randomCustomers, deposit);
				chormosomes.Add(chromosome);
			}

			return chormosomes;
		}

		public void Mutate(List<Chromosome> children)
		{
			var rand = StaticRandom.Instance;
			var rate = GaParameters.MutationRate * 100;
			var maxValue = children.Count - 1;

			foreach (var child in children)
			{
				var chance = rand.Next(1, 101);
				if (chance <= rate)
				{
					var firstPos = rand.Next(maxValue);
					var secondPos = rand.Next(maxValue);
					while (secondPos == firstPos)
					{
						secondPos = rand.Next(maxValue);
					}

					var tmp = child.Customers[firstPos];
					child.Customers[firstPos] = child.Customers[secondPos];
					child.Customers[secondPos] = tmp;
				}
			}
		}

		public List<Chromosome> DoTournament(bool returnSorted = true)
		{
			var children = new List<Chromosome>();

			for (int popIndex = 0; popIndex < GaParameters.PopulationSize / 2; popIndex++)
			{
				var parent1 = ComputeFitnessAndOrder(Parents.GetRandomList(GaParameters.TournamentSize)).First();
				var parent2 = ComputeFitnessAndOrder(Parents.GetRandomList(GaParameters.TournamentSize)).First();

				var crossOverChildren = PerformCrossover(GaParameters.CrossoverType, new List<Chromosome>()
				{
					parent1,
					parent2
				});

				children.AddRange(crossOverChildren);
			}

			if (returnSorted)
				children = ComputeFitnessAndOrder(children);

			return children;
		}

		public double GetAverageFitness(List<Chromosome> chromosomes)
		{
			return chromosomes.Sum(x => x.Fitness) / chromosomes.Count;
		}

		public List<Chromosome> RemoveDuplicateChromosomes(List<Chromosome> children)
		{
			return children.GroupBy(x => x.Id).Select(x => x.First()).ToList();
		}

		public void PerformElitisim()
		{
			for (int i = 0; i < GaParameters.EliteCount - 1; i++)
			{
				Children.Add(sortedParents[i].Copy());
			}
		}
	}
}
