using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using GaVrp.Entities.Ga;
using GaVrp.Entities.Report;
using Newtonsoft.Json;

namespace GaVrp.Library
{
	public class ReportGenerator
	{
		private const string outputPath = "VrptwTestResults";

		public ReportGenerator(bool cleanUp = true)
		{
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			System.IO.DirectoryInfo di = new DirectoryInfo(outputPath);
			if (cleanUp)
			{
				foreach (FileInfo file in di.GetFiles())
				{
					file.Delete();
				}
			}
		}

		public void SaveGenerationReports(List<GenerationReport> generationReports, int index)
		{
			using (TextWriter writer = new StreamWriter(Path.Combine(outputPath, $"GenerationReports-{index}.csv")))
			{
				var csv = new CsvWriter(writer);
				csv.WriteRecords(generationReports);
			}
		}

		public void SaveProgramReports(List<ProgramReport> programRunReports)
		{
			using (TextWriter writer = new StreamWriter(Path.Combine(outputPath, $"ProgramRunReports.csv")))
			{
				var csv = new CsvWriter(writer);
				csv.WriteRecords(programRunReports);
			}
		}

		public void SaveParameters(string fileName)
		{
			var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(new
			{
				VehicleSpeed = GaParameters.VehicleSpeed,
				VehicleWeight = GaParameters.VehicleWeight,
				DistanceWeight = GaParameters.DistanceWeight,
				GenerationCount = GaParameters.GenerationCount,
				PopulationSize = GaParameters.PopulationSize,
				MutationRate = GaParameters.MutationRate,
				CrossOverRate = GaParameters.CrossOverRate,
				EliteCount = GaParameters.EliteCount,
				TournamentSize = GaParameters.TournamentSize,
				VehicleCapacity = GaParameters.VehicleCapacity,
				CrossoverType = GaParameters.CrossoverType,
				FitnessMethod = GaParameters.FitnessMethod
			},
			Formatting.Indented,
			new Newtonsoft.Json.JsonSerializerSettings()
			{
				Converters = new List<Newtonsoft.Json.JsonConverter> {
					new Newtonsoft.Json.Converters.StringEnumConverter()
				}
			});

			File.WriteAllText(Path.Combine(outputPath, fileName + ".json"), parameters);
		}
	}
}
