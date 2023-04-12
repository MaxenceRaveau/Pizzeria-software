using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Pizzeria.Database;

namespace Pizzeria {
	public partial class NewWorkerWindow {
		private static List<string> Vehicles = new List<string> {"Vélo", "Moto", "Voiture"};

		private readonly Employee[] _employee;

		public NewWorkerWindow(Employee[] employee) {
			InitializeComponent();
			_employee = employee;
			
			foreach (var vehicle in Vehicles) {
				VehicleComboBox.Items.Add(vehicle);
			}

			VehicleComboBox.SelectedIndex = 0;
		}

		private void SaveButtonClicked(object sender, RoutedEventArgs e) {
			if (DeliveryManButton.IsEnabled)
				_employee[0] = new Helper {Name = NameWorkerTextBox.Text, HiringDate = DateTime.Now};
			else _employee[0] = new DeliveryMan {Name = NameWorkerTextBox.Text, Vehicle = VehicleComboBox.Text};

			Close();
		}

		private void EmployeeTypeButton_OnClick(object sender, RoutedEventArgs e) {
			HelperButton.IsEnabled = true;
			DeliveryManButton.IsEnabled = true;
			((Button) sender).IsEnabled = false;
		}

		private void NameWorkerTextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
			RegisterWorkerButton.IsEnabled = NameWorkerTextBox.Text.Length != 0;
		}
	}
}