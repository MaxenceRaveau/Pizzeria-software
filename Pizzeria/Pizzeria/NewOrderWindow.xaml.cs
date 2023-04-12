using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Pizzeria.Database;

namespace Pizzeria {
	public partial class NewOrderWindow {
		private static readonly IReadOnlyList<(string, int)> PizzaSizes = new List<(string, int)> {
			("Petite", 0), ("Moyenne", 3), ("Grande", 5)
		};

		private static readonly IReadOnlyList<(string, int)> PizzaRecipes = new List<(string, int)> {
			("Margherita", 10), ("4 Fromaggi", 14), ("Barese", 14), ("Calzone", 12), ("Tonnina", 12),
			("Regina", 12), ("4 Stagioni", 14)
		};

		private static readonly IReadOnlyList<(string, int)> Drinks = new List<(string, int)> {
			("Eau plate", 2), ("Eau gazeuse", 3), ("Jus de pomme infusion Basilic", 6)
		};

		private readonly Order[] _order;
		private readonly Order _builtOrder;
		private readonly Database.Database _database;

		private readonly List<Helper> _helpers;

		public NewOrderWindow(int orderNumber, Order[] order, Database.Database database) {
			InitializeComponent();

			_order = order;
			_builtOrder = new Order {
				OrderDate = DateTime.Now,
				OrderedProducts = new List<string>(),
				OrderNumber = orderNumber,
				Client = null
			};
			_database = database;

			CommandNumberLabel.Content = orderNumber.ToString("D16");
			DateLabel.Content = DateTime.Now.Date.ToString(CultureInfo.CurrentCulture);

			_helpers = new List<Helper>();
			foreach (var employee in _database.Employees) {
				if (employee is Helper helper) _helpers.Add(helper);
			}
			foreach (var helper in _helpers) {
				HelpersComboBox.Items.Add(helper.Name);
			}

			HelpersComboBox.SelectedIndex = 0;

			PizzaSize.Items.Add("-- Taille de pizza --");
			foreach (var (pizzaSize, additionalPrice) in PizzaSizes) {
				PizzaSize.Items.Add(pizzaSize + " (+" + additionalPrice + " €)");
			}

			PizzaSize.SelectedIndex = 0;

			PizzaRecipe.Items.Add("-- Type de pizza --");
			foreach (var (pizzaName, price) in PizzaRecipes) {
				PizzaRecipe.Items.Add(pizzaName + " (" + price + " €)");
			}

			PizzaRecipe.SelectedIndex = 0;

			DrinkSelector.Items.Add("-- Boissons --");
			foreach (var (drink, price) in Drinks) {
				DrinkSelector.Items.Add(drink + " (" + price + " €)");
			}

			DrinkSelector.SelectedIndex = 0;
		}

		private void PizzaSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			PizzaAddToCartButton.IsEnabled = PizzaRecipe.SelectedIndex > 0 && PizzaSize.SelectedIndex > 0;
		}

		private void PizzaAddToCartButton_OnClick(object sender, RoutedEventArgs e) {
			var (pizza, pizzaPrice) = PizzaRecipes[PizzaRecipe.SelectedIndex - 1];
			var (pizzaSize, pizzaSizeAdditionalCost) = PizzaSizes[PizzaSize.SelectedIndex - 1];

			_builtOrder.Price += pizzaPrice + pizzaSizeAdditionalCost;
			_builtOrder.OrderedProducts.Add(pizzaSize + ' ' + pizza);

			PizzaRecipe.SelectedIndex = 0;
			PizzaSize.SelectedIndex = 0;
			PizzaAddToCartButton.IsEnabled = false;

			CheckOrderValidity();
		}

		private void DrinkSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			DrinkAddToCartButton.IsEnabled = DrinkSelector.SelectedIndex > 0;
		}

		private void DrinkAddToCartButton_OnClick(object sender, RoutedEventArgs e) {
			var (drink, price) = Drinks[DrinkSelector.SelectedIndex - 1];

			_builtOrder.Price += price;
			_builtOrder.OrderedProducts.Add(drink);

			DrinkSelector.SelectedIndex = 0;
			DrinkAddToCartButton.IsEnabled = false;

			CheckOrderValidity();
		}

		private void PhoneNumberTextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
			PhoneNumberButton.IsEnabled = PhoneNumberTextBox.Text.Length == 10 && PhoneNumberTextBox.Text[0] == '0' &&
			                              int.TryParse(PhoneNumberTextBox.Text, out _);
		}

		private void PhoneNumberButton_OnClick(object sender, RoutedEventArgs e) {
			var foundClientIndex =
				_database.Clients.FindIndex(client => string.Equals(client.PhoneNumber, PhoneNumberTextBox.Text));

			if (foundClientIndex == -1) {
				var newClient = new Client[1];
				new NewClientWindow(newClient, PhoneNumberTextBox.Text).ShowDialog();
				if (newClient[0] == null) return;
				_database.Clients.Add(newClient[0]);
				foundClientIndex = _database.Clients.Count - 1;
			}

			var client = _database.Clients[foundClientIndex];

			_builtOrder.Client = client;
			SelectedClientName.Content =
				"Client sélectionné : " + _builtOrder.Client.Name + ' ' + _builtOrder.Client.FirstName;

			CheckOrderValidity();
		}


		private void HelpersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			CheckOrderValidity();
		}

		private void CheckOrderValidity() {
			CompleteOrderButton.IsEnabled = _builtOrder.Price != 0 && _builtOrder.Client != null;
		}

		private void CompleteOrderButton_OnClick(object sender, RoutedEventArgs e) {
			//Store the order.
			_builtOrder.HelperName = HelpersComboBox.Text;
			_order[0] = _builtOrder;
			
			//Increment the number of orders handled by the employee.
			_helpers[HelpersComboBox.SelectedIndex].HandledOrderCount++;
			
			Close();
		}
	}
}