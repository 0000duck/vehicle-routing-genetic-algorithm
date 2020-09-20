using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaVrp.Entities.Ga;

namespace GaVrp.Entities.Map
{
	public class Map
	{
		public List<Customer> Customers { get; set; }
		public List<VehicleRoute> VehicleRoutes { get; set; } //Maybe later for UI
		public List<Node> Nodes { get; set; }
		public Deposit Deposit { get; set; }


		public double[,] DistanceMatrix;

		public Map(List<Customer> customers, Deposit deposit)
		{
			Nodes = new List<Node>();
			Customers = new List<Customer>();
			foreach (var customer in customers)
			{
				customer.Map = this;
				Nodes.Add(customer);
				Customers.Add(customer);
			}

			deposit.Map = this;
			Nodes.Add(deposit);
			Deposit = deposit;
			DistanceMatrix = new double[Nodes.Count + 1, Nodes.Count + 1]; //because ids start from 1
		}
	}
}
