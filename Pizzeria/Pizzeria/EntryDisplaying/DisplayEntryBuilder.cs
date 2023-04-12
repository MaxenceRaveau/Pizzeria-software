using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pizzeria.EntryDisplaying {
	public class DisplayEntryBuilder {
		private readonly Grid _displayGrid;

		public DisplayEntryBuilder(Grid displayGrid) {
			_displayGrid = displayGrid;
		}

		public void AddDisplayableToGrid(IDisplayableDataStructure displayableDataStructure) {
			var innerGrid = new Grid {Margin = new Thickness(5, 5, 5, 5)};
			AddDisplayedElements(innerGrid, displayableDataStructure);
			AddStateButtons(innerGrid, displayableDataStructure);

			var border = new Border {
				Background = new SolidColorBrush(Colors.Gray),
				BorderBrush = new SolidColorBrush(Colors.Black),
				BorderThickness = new Thickness(1),
				CornerRadius = new CornerRadius(3),
				Child = innerGrid
			};

			var rowDefinition = new RowDefinition {Height = GridLength.Auto};
			_displayGrid.RowDefinitions.Add(rowDefinition);
			_displayGrid.Children.Add(border);
			border.SetValue(Grid.RowProperty, _displayGrid.RowDefinitions.Count - 1);
		}

		private static void AddDisplayedElements(Grid innerGrid, IDisplayableDataStructure displayableDataStructure) {
			var displayedElements = displayableDataStructure.GetDisplayedElements();

			//Add the elements as labels, each in a new column in the grid.
			for (var i = 0; i < displayedElements.Count; ++i) {
				innerGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});

				var newLabel = new Label {Content = displayedElements[i]};
				innerGrid.Children.Add(newLabel);
				newLabel.SetValue(Grid.ColumnProperty, i);
			}
		}

		private static void AddStateButtons(Grid innerGrid, IDisplayableDataStructure displayableDataStructure) {
			var buttonNames = displayableDataStructure.GetStates();
			var buttons = buttonNames.Select(bn => new Button {
				Content = bn,
				Background = new SolidColorBrush(Colors.DimGray),
				Margin = new Thickness(1, 1, 1, 1),
				BorderBrush = new SolidColorBrush(Colors.Black),
				BorderThickness = new Thickness(1)
			}).ToList();
			buttons[0].IsEnabled = false;

			//Make a grid for the buttons. This grid will be displayed on the right of the innerGrid.
			var buttonsGrid = new Grid {HorizontalAlignment = HorizontalAlignment.Right};
			for (var i = 0; i < buttons.Count; i++) {
				//Add the button to the grid of buttons.
				buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
				buttonsGrid.Children.Add(buttons[i]);
				buttons[i].SetValue(Grid.ColumnProperty, i);

				//Add a Click event delegate that enables all buttons and disables this one.
				buttons[i].Click += (sender, args) => {
					foreach (var button in buttons) {
						button.IsEnabled = true;
					}

					((Button) sender).IsEnabled = false;
				};
			}

			//Add the button grid on the right of the inner grid.
			innerGrid.ColumnDefinitions.Add(new ColumnDefinition());
			innerGrid.Children.Add(buttonsGrid);
			buttonsGrid.SetValue(Grid.ColumnProperty, innerGrid.ColumnDefinitions.Count - 1);
		}
	}
}