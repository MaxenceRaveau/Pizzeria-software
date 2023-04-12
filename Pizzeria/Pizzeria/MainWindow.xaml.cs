using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using Pizzeria.Database;
using Pizzeria.EntryDisplaying;

namespace Pizzeria {
	public partial class MainWindow {
		private Database.Database _database;

		public MainWindow() {
			InitializeComponent();

			try {
				Stream stream = File.Open("./database", FileMode.Open);
				var binaryFormatter = new BinaryFormatter();
				_database = (Database.Database) binaryFormatter.Deserialize(stream);
				stream.Close();
			}
			catch (FileNotFoundException) {
				_database = new Database.Database {
					OrderCount = 0,
					Clients = new List<Client>(),
					Orders = new List<Order>(),
					Employees = new List<Employee>()
				};
			}

			if (Application.Current.MainWindow != null)
				Application.Current.MainWindow.WindowState = WindowState.Maximized;

			var ordersEntryBuilder = new DisplayEntryBuilder(OrdersGrid);
			foreach (var order in _database.Orders) {
				ordersEntryBuilder.AddDisplayableToGrid(order);
			}

			var employeesEntryBuilder = new DisplayEntryBuilder(EmployeesGrid);
			foreach (var employee in _database.Employees) {
				employeesEntryBuilder.AddDisplayableToGrid(employee);
			}
		}

		private void MainWindowClosing(object sender, CancelEventArgs e) {
			Stream stream = File.Open("./database", FileMode.Create);
			var binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(stream, _database);
			stream.Close();
		}

		private void NewOrderClicked(object sender, RoutedEventArgs e) {
			//Can't create a command if there's no helper.
			if (!_database.Employees.Any(employee => employee is Helper)) {
				MessageBox.Show("Il faut au moins un commis pour créer une commande.", "Erreur", MessageBoxButton.OK);
				return;
			}

			//Get a new order from the user.
			var newOrder = new Order[1];
			new NewOrderWindow(_database.OrderCount, newOrder, _database).ShowDialog();
			if (newOrder[0] == null) return;
			_database.Orders.Add(newOrder[0]);

			//Display the new order.
			var displayEntryBuilder = new DisplayEntryBuilder(OrdersGrid);
			displayEntryBuilder.AddDisplayableToGrid(newOrder[0]);

			//Increment the total order count.
			_database.OrderCount++;
		}

		private void FindClientTextEntered(object sender, TextChangedEventArgs e) {
			if (FindClientTextBox.Text.Length == 10 && FindClientTextBox.Text[0] == '0' &&
			    int.TryParse(FindClientTextBox.Text, out _)) {
				FindClientButton.IsEnabled = true;
				FindClientButton.Content = "Rechercher";
			}
			else {
				FindClientButton.IsEnabled = false;
				FindClientButton.Content = "Entrez un numéro valide";
			}
		}

		private void FindClientButtonClick(object sender, RoutedEventArgs e) {
			//Check if a registered client has this number.
			var foundClientIndex =
				_database.Clients.FindIndex(c => string.Equals(c.PhoneNumber, FindClientTextBox.Text));

			//If not, launch a window to register this number as a new client.
			if (foundClientIndex == -1) {
				var newClient = new Client[1];
				new NewClientWindow(newClient, FindClientTextBox.Text).ShowDialog();
				if (newClient[0] == null) return;
				_database.Clients.Add(newClient[0]);
				foundClientIndex = _database.Clients.Count - 1;
			}

			var client = _database.Clients[foundClientIndex];

			ClientName.Content = client.Name;
			ClientFirstName.Content = client.FirstName;
			ClientPhoneNumber.Content = client.PhoneNumber;
			ClientAddress1.Content = client.Address1;
			ClientAddress2.Content = client.Address2;
			ClientOrderCount.Content = "Ce client a passé " + client.CommandCount + " commandes ici";
		}

		private void NewEmployeeButton_OnClick(object sender, RoutedEventArgs e) {
			//Launch a window where the user can create a new employee.
			var newEmployee = new Employee[1];
			new NewWorkerWindow(newEmployee).ShowDialog();
			if (newEmployee[0] == null) return;
			_database.Employees.Add(newEmployee[0]);

			//Display the new employee.
			var displayEntryBuilder = new DisplayEntryBuilder(EmployeesGrid);
			displayEntryBuilder.AddDisplayableToGrid(newEmployee[0]);
		}
	}
}