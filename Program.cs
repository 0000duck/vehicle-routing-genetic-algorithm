using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaVrp.Core;
using GaVrp.Entities;
using GaVrp.Entities.Ga;
using GaVrp.Entities.Map;
using GaVrp.Entities.Report;
using GaVrp.Library;
using GaVrp.Library.GA;

namespace GaVrp
{
	class Program
	{
		static void Main(string[] args)
		{
			Logger.Log("Process Started....");
			Logger.AddSpace();

			try
			{
				Logger.Log("Loading Maps (Data)...");

				var vrpCore = new VrpCore();
				var maps = vrpCore.LoadMaps();

				var reportGenerator = new ReportGenerator();

				var programRunReports = new List<ProgramReport>();
				for (var index = 0; index < GaParameters.ProgramRunCount * TestFlow.TotalStages; index++)
				{
					Logger.Log($"Starting Program Run: {index}");
					Logger.Log($"Processing, Please wait...");

					if (GaParameters.TestFlow)
					{
						TestFlow.ChangeParameters(index);
					}

					var sw = new Stopwatch();
					sw.Start();
					var generationReports = vrpCore.GetBestResults(maps[0]); //or could be maps[i] to work with different map for each run
					sw.Stop();
					var es = sw.Elapsed.Seconds;

					var bestOfGenerations = generationReports.OrderBy(x => x.BestFitnessValue).First();
					var worstOfGenerations = generationReports.OrderByDescending(x => x.WorstFitnessValue).First();
					var programReport = new ProgramReport()
					{
						BestFitnessValue = bestOfGenerations.BestFitnessValue,
						WorstFitnessValue = worstOfGenerations.WorstFitnessValue,
						BestResult = bestOfGenerations.BestResult.Substring(1) //remove first dash
					};

					if (TestFlow.ExperimentStageDone(index))
					{
						programRunReports.Add(programReport);
						reportGenerator.SaveGenerationReports(generationReports, TestFlow.StageCount);
						reportGenerator.SaveParameters("Parameters-" + TestFlow.StageCount.ToString());
					}

				}

				reportGenerator.SaveProgramReports(programRunReports);
				Logger.Log("Reports Saved.");
				Logger.Log("Finished.");


			}
			catch (Exception ex)
			{
				if (System.Diagnostics.Debugger.IsAttached)
				{
					throw;
				}
				else
				{
					Console.WriteLine(ex);
				}
			}
			finally
			{
				Console.ReadLine();
			}

		}
	}
}
