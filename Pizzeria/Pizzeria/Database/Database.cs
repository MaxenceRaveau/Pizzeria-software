using System;
using System.Collections.Generic;

namespace Pizzeria.Database {
	[Serializable]
	public struct Database {
		public int OrderCount { get; set; }
		public List<Client> Clients { get; set; }
		public List<Order> Orders { get; set; }
		public List<Employee> Employees { get; set; }
	}
}