using System;
using System.Windows;
using Pizzeria.Database;

namespace Pizzeria {
	public partial class NewClientWindow {
		private Client[] _client;
		
		public NewClientWindow(Client[] client, string phoneNumber) {
			InitializeComponent();
			_client = client;
			PhoneNumber.Text = phoneNumber;
		}

		private void SaveButtonClicked(object sender, RoutedEventArgs e) {
			if (!string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Prenom.Text) &&
			    !string.IsNullOrEmpty(PhoneNumber.Text) && !string.IsNullOrEmpty(Adresse1.Text) &&
			    !string.IsNullOrEmpty(Ville.Text)) {
				_client[0] = new Client {
					Name = Nom.Text,
					FirstName = Prenom.Text,
					PhoneNumber = PhoneNumber.Text,
					FirstCommandName = DateTime.Today,
					Address1 = Adresse1.Text,
					Address2 = Adresse2.Text,
					City = Ville.Text,
					CommandCount = 0
				};
				Close();
			}
		}
	}
}