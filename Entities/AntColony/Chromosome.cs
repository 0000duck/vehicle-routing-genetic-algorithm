using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GaVrp.Entities.Map;
using GaVrp.Library.GA;

namespace GaVrp.Entities.Ga
{
	public class Chromosome
	{
		private StringBuilder id;
		public string Id
		{
			get
			{
				if (id == null)
				{
					id = new StringBuilder();
					foreach (var customer in Customers)
					{
						id.Append("-");
						id.Append(customer.Id);
					}
				}
				return id.ToString();
			}
		}

		private List<VehicleRoute> vehicleRoutes;
		public List<VehicleRoute> VehicleRoutes
		{
			get
			{
				if (vehicleRoutes == null)
				{
					vehicleRoutes = CalculateVehicleRoutes();
				}

				return vehicleRoutes;
			}
		}

		public int VehiclesNeeded
		{
			get { return VehicleRoutes.Count; }
		}

		public List<Customer> Customers { get; set; }
		public Deposit Deposit { get; set; }

		public double Fitness { get; set; }
		public double Rank { get; set; }

		public Chromosome(List<Customer> customers, Deposit deposit)
		{
			Customers = customers;
			Deposit = deposit;
		}

		public double GetTotalDistance()
		{
			double distance = 0;
			foreach (var route in VehicleRoutes)
			{
				distance = distance + route.GetTotalDistance();
			}

			return distance;
		}

		public List<VehicleRoute> CalculateVehicleRoutes()
		{
			var routes = new List<VehicleRoute>();

			var route = new VehicleRoute(Deposit);
			route.InitializeRoute();

			for (int i = 0; i < Customers.Count - 1; i++)
			{
				route.InsertCustomer(Customers[i]);

				if (!route.CanNextCustomerBeAdded(Customers[i + 1]))
				{
					route.EndRoute();
					routes.Add(route);
					route = new VehicleRoute(Deposit);
					route.InitializeRoute();
				}

				if (i == Customers.Count - 1)
				{
					route.EndRoute();
				}
			}

			return routes;
		}


		double fittness = -1; // for effeciency
		public double CalculateWeightedSum()
		{
			if (fittness == -1)
				fittness = GaParameters.DistanceWeight * this.GetTotalDistance() + GaParameters.VehicleWeight * VehiclesNeeded;
			return fittness;
		}

		public double GetParetoRank(ParetoRanking<string> paretoRanking)
		{
			return paretoRanking.GetRanking(Id);
		}

		public ParetoRanking<string> ComputeParetoRank(IList<Chromosome> chromosomes)
		{
			var paretoRanking = new ParetoRanking<string>();
			var distinctChromosomes = chromosomes.GroupBy(x => x.Id).Select(x => x.First());
			foreach (var chromosome in distinctChromosomes)
			{
				paretoRanking.AddVector(chromosome.Id, chromosome.VehiclesNeeded, chromosome.GetTotalDistance());
			}
			paretoRanking.ComputeRanking();

			return paretoRanking;
		}

		public Chromosome Copy()
		{
			var customers = new List<Customer>();
			foreach (var customer in this.Customers)
			{
				customers.Add(customer.Copy());
			}

			var deposit = new Deposit();
			return new Chromosome(customers, deposit);
		}

		/// <summary>
		/// just for test
		/// </summary>
		/// <returns></returns>
		public bool ValidateUniqueCustomers()
		{
			var cCount = Customers.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Count();
			return cCount == Customers.Count();
		}
	}
}
