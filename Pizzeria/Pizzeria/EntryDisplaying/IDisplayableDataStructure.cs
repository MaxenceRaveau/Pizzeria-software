using System.Collections.Generic;

namespace Pizzeria.EntryDisplaying {
	public interface IDisplayableDataStructure {
		/// <summary>
		/// Returns strings meant to be displayed as is, in the element's ui section.
		/// </summary>
		public List<string> GetDisplayedElements();

		/// <summary>
		/// Returns the names of the state this element can be in. The user should be able to switch between one another
		/// by clicking on buttons.
		/// </summary>
		public List<string> GetStates();
	}
}