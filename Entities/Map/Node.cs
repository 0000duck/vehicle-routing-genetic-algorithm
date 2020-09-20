using System;
using GaVrp.Entities.Ga;

namespace GaVrp.Entities.Map
{
	public class Node
	{
		public int Id { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public Map Map { get; set; }

		public double GetDistance(Node destination)
		{
			if (Map.DistanceMatrix[this.Id, destination.Id] == 0)
			{
				var xDistance = X - destination.X;
				var yDistance = Y - destination.Y;
				var distance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
				Map.DistanceMatrix[this.Id, destination.Id] = distance;
				return distance;
			}

			return Map.DistanceMatrix[this.Id, destination.Id];
		}

		public double GetTravelingTime(Node destination)
		{
			return GetDistance(destination) / GaParameters.VehicleSpeed;
		}
	}
}
