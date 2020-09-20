namespace GaVrp.Entities.Map
{
	public class Customer : Node
	{
		public float Demand { get; set; }
		public float ReadyTime { get; set; }
		public float DueDate { get; set; }
		public float ServiceTime { get; set; }


		public Customer Copy()
		{
			return new Customer()
			{
				Id = Id,
				X = X,
				Y = Y,
				Demand = this.Demand,
				ReadyTime = this.ReadyTime,
				DueDate = this.DueDate,
				ServiceTime = ServiceTime
			};
		}
	}
}
