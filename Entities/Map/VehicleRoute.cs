using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaVrp.Entities.Ga;

namespace GaVrp.Entities.Map
{
	public class VehicleRoute
	{
		public List<Node> RouteNodesList { get; set; }

		public Deposit Deposit { get; set; }

		public float Capacity { get; set; }

		public float ElapsedTime { get; set; }

		public VehicleRoute(Deposit deposit)
		{
			Deposit = deposit;
			RouteNodesList = new List<Node>();

		}

		public void InitializeRoute()
		{
			RouteNodesList.Add(Deposit);
		}

		public void EndRoute()
		{
			RouteNodesList.Add(Deposit);
		}

		public void InsertCustomer(Customer customer)
		{
			Capacity += customer.Demand;
			var travelTime = RouteNodesList.Last().GetTravelingTime(customer);

			ElapsedTime = (float) (ElapsedTime + customer.ServiceTime + travelTime);
			RouteNodesList.Add(customer);
		}

		public double GetTotalDistance()
		{
			double totalDistance = 0;
			for (int i = 0; i < RouteNodesList.Count - 1; ++i)
			{
				totalDistance += RouteNodesList[i].GetDistance(RouteNodesList[i + 1]);
			}
			return totalDistance;
		}


		/// <summary>
		/// The method here is very basic and effects the overall performance and could be improved later.
		/// </summary>
		/// <param name="nextCustomer"></param>
		/// <returns></returns>
		public bool CanNextCustomerBeAdded(Customer nextCustomer)
		{
			var depositTravelTime = nextCustomer.GetDistance(Deposit);
			var nextCustomerTravelTime = ElapsedTime + RouteNodesList.Last().GetTravelingTime(nextCustomer);

			//if (Deposit.DueDate < depositTravelTime)
			//{
			//	return false;
			//}

			if (Capacity + nextCustomer.Demand > GaParameters.VehicleCapacity)
			{
				return false;
			}

			//if (nextCustomerTravelTime < nextCustomer.DueDate && nextCustomerTravelTime > nextCustomer.ReadyTime) //should arrive in date range
			//{
			//	return false;
			//}

			return true;
		}
	}
}
