using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GaVrp.Library.GA;

namespace GaVrp.Entities.Ga
{
	public static class GaParameters
	{
		private static string Config(string value)
		{
			return ConfigurationManager.AppSettings[value];
		}


		private static float speed;
		public static float VehicleSpeed
		{
			get
			{
				if (Math.Abs(speed) <= 0)
				{
					speed = float.Parse(Config("VehicleSpeed"));
				}
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		private static float vehicleWeight;
		public static float VehicleWeight
		{
			get
			{
				if (Math.Abs(vehicleWeight) <= 0)
				{
					vehicleWeight = float.Parse(Config("VehicleWeight"));
				}
				return vehicleWeight;
			}
			set
			{
				vehicleWeight = value;
			}
		}

		private static float distanceWeight;
		public static float DistanceWeight
		{
			get
			{
				if (Math.Abs(distanceWeight) <= 0)
				{
					distanceWeight = float.Parse(Config("DistanceWeight"));
				}
				return distanceWeight;
			}
			set
			{
				distanceWeight = value;
			}
		}

		private static int generationCount;
		public static int GenerationCount
		{
			get
			{
				if (Math.Abs(generationCount) <= 0)
				{
					generationCount = int.Parse(Config("GenerationCount"));
				}
				return generationCount;
			}
			set
			{
				generationCount = value;
			}
		}

		private static int populationSize;
		public static int PopulationSize
		{
			get
			{
				if (Math.Abs(populationSize) <= 0)
				{
					populationSize = int.Parse(Config("PopulationSize"));
				}
				return populationSize;
			}
			set
			{
				populationSize = value;
			}
		}

		private static float mutationRate;
		public static float MutationRate
		{
			get
			{
				if (Math.Abs(mutationRate) <= 0)
				{
					mutationRate = float.Parse(Config("MutationRate"));
				}
				return mutationRate;
			}
			set
			{
				mutationRate = value;
			}
		}

		private static float crossOverRate;
		public static float CrossOverRate
		{
			get
			{
				if (Math.Abs(crossOverRate) <= 0)
				{
					crossOverRate = float.Parse(Config("CrossOverRate"));
				}
				return crossOverRate;
			}
			set
			{
				crossOverRate = value;
			}
		}

		private static int eliteCount;
		public static int EliteCount
		{
			get
			{
				if (Math.Abs(eliteCount) <= 0)
				{
					eliteCount = int.Parse(Config("EliteCount"));
				}
				return eliteCount;
			}
			set
			{
				eliteCount = value;
			}
		}


		private static int tournamentSize;
		public static int TournamentSize
		{
			get
			{
				if (Math.Abs(tournamentSize) <= 0)
				{
					tournamentSize = int.Parse(Config("TournamentSize"));
				}
				return tournamentSize;
			}
			set
			{
				tournamentSize = value;
			}
		}

		private static int programRunCount;
		public static int ProgramRunCount
		{
			get
			{
				if (Math.Abs(programRunCount) <= 0)
				{
					programRunCount = int.Parse(Config("ProgramRunCount"));
				}
				return programRunCount;
			}
			set
			{
				programRunCount = value;
			}
		}

		private static int vehicleCapacity;
		public static int VehicleCapacity
		{
			get
			{
				if (Math.Abs(vehicleCapacity) <= 0)
				{
					vehicleCapacity = int.Parse(Config("VehicleCapacity"));
				}
				return vehicleCapacity;
			}
			set
			{
				vehicleCapacity = value;
			}
		}

		private static string benchmarkFolderPath;
		private static string BenchmarkFolderPath
		{
			get
			{
				if (string.IsNullOrEmpty(benchmarkFolderPath))
				{
					benchmarkFolderPath = Config("BenchmarkFolderPath");
				}
				return benchmarkFolderPath;
			}
			set
			{
				benchmarkFolderPath = value;
			}
		}

		private static List<string> inputDataList = new List<string>();
		public static List<string> FilesPathList
		{
			get
			{
				if (!inputDataList.Any())
				{
					foreach (var filePath in Config("FilesPathList").Split(';').ToList())
					{
						inputDataList.Add(BenchmarkFolderPath + filePath);
					}
				}
				return inputDataList;
			}
			set
			{
				inputDataList = value;
			}
		}


		private static CrossoverType crossoverType;
		public static CrossoverType CrossoverType
		{
			get
			{
				if (crossoverType == Library.GA.CrossoverType.None)
				{
					var value = int.Parse(Config("CrossoverType"));
					crossoverType = (Library.GA.CrossoverType)value;
				}
				return crossoverType;
			}
			set
			{
				crossoverType = value;
			}
		}

		private static FitnessMethod fitnessMethod;
		public static FitnessMethod FitnessMethod
		{
			get
			{
				if (fitnessMethod == Library.GA.FitnessMethod.None)
				{
					var value = int.Parse(Config("FitnessMethod"));
					fitnessMethod = (Library.GA.FitnessMethod)value;
				}
				return fitnessMethod;
			}
			set
			{
				fitnessMethod = value;
			}
		}

		private static bool? testFlow;
		public static bool TestFlow
		{
			get
			{
				if (testFlow == null)
				{
					testFlow = bool.Parse(Config("TestFlow"));
				}
				return testFlow.Value;
			}
			set
			{
				testFlow = value;
			}
		}
	}
}
