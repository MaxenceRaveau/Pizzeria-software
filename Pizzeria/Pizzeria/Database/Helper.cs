using System;
using System.Collections.Generic;
using System.Globalization;

namespace Pizzeria.Database {
	[Serializable]
	public class Helper : Employee {
		public DateTime HiringDate { get; set; }
		public int HandledOrderCount { get; set; }

		public override List<string> GetDisplayedElements() {
			return new List<string> {
				"Commis",
				Name,
				"Commandes gérées : " + HandledOrderCount,
				"Date d'embauche : " + HiringDate.ToString(CultureInfo.CurrentCulture)
			};
		}

		public override List<string> GetStates() {
			return new List<string> {"Sur place", "En congés"};
		}
	}
}