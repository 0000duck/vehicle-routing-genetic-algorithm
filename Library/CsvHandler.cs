using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GaVrp.Entities;
using GaVrp.Entities.Map;

namespace GaVrp.Library
{
	public class CsvHandler
	{
		public Map LoadCustomersAndDepo(string filePath)
		{
			using (TextReader fileReader = File.OpenText(filePath))
			{
				var csv = new CsvReader(fileReader);

				var mapping = new Factory().CreateClassMapBuilder<Customer>()
					.Map(m => m.Id).Name("CUST_NO")
					.Map(m => m.X).Name("XCOORD")
					.Map(m => m.Y).Name("YCOORD")
					.Map(m => m.Demand).Name("DEMAND")
					.Map(m => m.ReadyTime).Name("READY_TIME")
					.Map(m => m.DueDate).Name("DUE_DATE")
					.Map(m => m.ServiceTime).Name("SERVICE_TIME")
					.Build();
				csv.Configuration.RegisterClassMap(mapping);

				var customersRecords = csv.GetRecords<Customer>().ToList();
				var firstCustomer = customersRecords.First();

				var customers = customersRecords.Skip(1).ToList(); //first record is the deposit
				var deposit = new Deposit()
				{
					DueDate = firstCustomer.DueDate,
					X = firstCustomer.X,
					Y = firstCustomer.Y,
					Id = firstCustomer.Id
				};

				var map = new Map(customers, deposit);

				return map;
			}
		}

	}
}
