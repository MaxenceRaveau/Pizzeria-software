using System;
using System.Collections.Generic;
using Pizzeria.EntryDisplaying;

namespace Pizzeria.Database {
	[Serializable]
	public class Order : IDisplayableDataStructure {
		public int OrderNumber { get; set; }
		public DateTime OrderDate { get; set; }
		public Client Client { get; set; }
		public string HelperName { get; set; }
		public List<string> OrderedProducts { get; set; }
		public int Price { get; set; }

		public List<string> GetDisplayedElements() {
			return new List<string> {
				"Numéro de commande : " + OrderNumber.ToString("D16"),
				"Addresse du client : " + Client.Address1,
				"Cout de la commande : " + Price + "€"
			};
		}

		public List<string> GetStates() {
			return new List<string> {"En préparation", "En livraison", "Complétée"};
		}
	}
}