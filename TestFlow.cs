using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaVrp.Entities.Ga;
using GaVrp.Library.GA;

namespace GaVrp
{
	public class TestFlow
	{
		public static int TotalStages = 1;
		public static int StageCount; //every totalRunsPerTest is one stage
		public static void ChangeParameters(int index)
		{
			if (ExperimentStageDone(index))
				StageCount++;
			else
			{
				return;
			}

			if (StageCount == 1)
			{
				//Use the defaults in app.config
				return;
			}

			if (StageCount == 2)
			{
				GaParameters.CrossoverType = CrossoverType.Pmx;
				return;
			}

			if (StageCount == 3)
			{
				GaParameters.CrossoverType = CrossoverType.Cx;
				return;
			}

			if (StageCount == 4)
			{
				GaParameters.CrossoverType = CrossoverType.Uox;
				GaParameters.PopulationSize = 500;
				return;
			}

			if (StageCount == 5)
			{
				GaParameters.CrossoverType = CrossoverType.Uox;
				return;
			}

			if (StageCount == 6)
			{
				GaParameters.CrossOverRate = (float) 0.6;
				return;
			}

			if (StageCount == 7)
			{
				GaParameters.CrossOverRate = (float)0.8;
				GaParameters.MutationRate = (float)0.4;
				return;
			}


			if (StageCount == 8)
			{
				GaParameters.MutationRate = (float)0.2;
				GaParameters.PopulationSize = 25;
				return;
			}

			if (StageCount == 9)
			{
				GaParameters.PopulationSize = 50;
				return;
			}

			if (StageCount == 10)
			{
				GaParameters.PopulationSize = 100;
				return;
			}

			if (StageCount == 11)
			{
				GaParameters.PopulationSize = 250;
				return;
			}

			if (StageCount == 12)
			{
				GaParameters.PopulationSize = 500;
				return;
			}

			if (StageCount == 13)
			{
				GaParameters.EliteCount = 0;
				return;
			}

			if (StageCount == 14)
			{
				GaParameters.EliteCount = 1;
				return;
			}

			if (StageCount == 15)
			{
				GaParameters.EliteCount = 5;
				return;
			}

			if (StageCount == 16)
			{
				GaParameters.EliteCount = 10;
				return;
			}

			if (StageCount == 17)
			{
				GaParameters.EliteCount = 2;
				GaParameters.FitnessMethod = FitnessMethod.Pareto;
				return;
			}

			if (StageCount == 18)
			{
				GaParameters.FitnessMethod = FitnessMethod.WeightedSum;
				return;
			}

		}

		public static bool ExperimentStageDone(int index)
		{
			return ((index + 1) % GaParameters.ProgramRunCount == 0);
		}
	}
}
