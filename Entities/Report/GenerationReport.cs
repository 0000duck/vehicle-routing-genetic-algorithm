using System;
using GaVrp.Entities.Ga;

namespace GaVrp.Entities.Report
{
	public class GenerationReport
	{
		public int GenerationNo { get; set; }

		public double BestFitnessValue { get; set; }

		public double WorstFitnessValue { get; set; }

		public string BestResult { get; set; }

		public double AverageFitnessValue { get; set; }

		public TimeSpan ProcessTime { get; set; }
	}
}
