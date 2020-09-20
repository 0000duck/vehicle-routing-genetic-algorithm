using System;
using GaVrp.Entities.Ga;

namespace GaVrp.Entities.Report
{
	public class ProgramReport
	{
		public double BestFitnessValue { get; set; }
		public double WorstFitnessValue { get; set; }
		public string BestResult { get; set; }
	}
}
