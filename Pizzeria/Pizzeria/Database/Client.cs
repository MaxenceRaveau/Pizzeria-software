using System;

namespace Pizzeria.Database {
	[Serializable]
	public class Client {
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime FirstCommandName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public int CommandCount { get; set; }
	}
}