using GaVrp.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaVrp.Entities.Ga;
using GaVrp.Entities.Map;
using GaVrp.Entities.Report;
using GaVrp.Library;
using GaVrp.Library.GA;

namespace GaVrp.Core
{

	public class VrpCore
	{
		private GaEngine gaEngine;
		public VrpCore()
		{
			gaEngine = new GaEngine();
		}

		public List<Map> LoadMaps()
		{
			var maps = new List<Map>();
			var csvHandler = new CsvHandler();
			foreach (var filePath in GaParameters.FilesPathList)
			{
				var map = csvHandler.LoadCustomersAndDepo(filePath);
				maps.Add(map);
			}

			return maps;
		}

		public List<GenerationReport> GetBestResults(Map map)
		{
			var initPopulation = gaEngine.InitializePopulation(map.Customers, map.Deposit);
			gaEngine.SetParentsAndResetChildren(initPopulation);
			var reports = new List<GenerationReport>();

			for (var generationIndex = 0; generationIndex < GaParameters.GenerationCount; generationIndex++)
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				var children = gaEngine.DoTournament(true).ToList();
				var bestChromosome = children.First(); // it is sorted
				var worstChromosome = children.Last(); 
				gaEngine.PerformElitisim();

				stopwatch.Stop();

				var report = new GenerationReport()
				{
					BestFitnessValue = bestChromosome.Fitness,
					BestResult = bestChromosome.Id,
					WorstFitnessValue = worstChromosome.Fitness,
					AverageFitnessValue = gaEngine.GetAverageFitness(children),
					GenerationNo = generationIndex,
					ProcessTime = stopwatch.Elapsed
				};

				reports.Add(report);

				children = gaEngine.RemoveDuplicateChromosomes(children);
				if (children.Count < 10)
					break;
				gaEngine.SetParentsAndResetChildren(children);
			}

			return reports;
		}
	}
}
