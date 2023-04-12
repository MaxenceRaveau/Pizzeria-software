using System;
using System.Collections.Generic;

namespace Pizzeria.Database {
	[Serializable]
	public class DeliveryMan : Employee {
		public string Vehicle { get; set; }

		public override List<string> GetDisplayedElements() {
			return new List<string> {
				"Livreur",
				Name,
				"Véhicule : " + Vehicle
			};
		}

		public override List<string> GetStates() {
			return new List<string> {"Sur place", "En livraison", "En congés"};
		}
	}
}