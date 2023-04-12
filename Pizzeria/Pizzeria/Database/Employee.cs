using System;
using System.Collections.Generic;
using Pizzeria.EntryDisplaying;

namespace Pizzeria.Database {
	[Serializable]
	public abstract class Employee : IDisplayableDataStructure {
		public string Name { get; set; }
		
		public abstract List<string> GetDisplayedElements();
		public abstract List<string> GetStates();
	}
}