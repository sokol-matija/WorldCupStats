using DataLayer;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_WorldCupStats
{
	public partial class InitialSettingsWindow : Window
	{
		private readonly IDataProvider _dataProvider;

		public InitialSettingsWindow(IDataProvider dataProvider)
		{
			InitializeComponent();
			_dataProvider = dataProvider;
			this.PreviewKeyDown += InitialSettingsWindow_PreviewKeyDown;
			LoadInitialSettings();
		}

		private async void LoadInitialSettings()
		{
			string championship = await _dataProvider.LoadSettingsAsync("Championship");
			string language = await _dataProvider.LoadSettingsAsync("Language");
			string windowSize = await _dataProvider.LoadSettingsAsync("WindowSize");

			if (!string.IsNullOrEmpty(championship) && cbChampionship != null && cbChampionship.Items.Count > 0)
			{
				var selectedItem = cbChampionship.Items.Cast<ComboBoxItem>()
					.FirstOrDefault(item => item.Tag != null && item.Tag.ToString() == championship);

				if (selectedItem != null)
				{
					cbChampionship.SelectedItem = selectedItem;
				}
			}
		}

		private async void SaveSettings_Click(object sender, RoutedEventArgs e)
		{
			await SaveSettings();
			this.DialogResult = true;
			this.Close();
		}

		private async Task SaveSettings()
		{
			string championship = (cbChampionship.SelectedItem as ComboBoxItem)?.Tag.ToString();
			string language = (cbLanguage.SelectedItem as ComboBoxItem)?.Tag.ToString();
			string windowSize = (cbWindowSize.SelectedItem as ComboBoxItem)?.Content.ToString();

			if (string.IsNullOrEmpty(championship) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(windowSize))
			{
				MessageBox.Show("Please select all settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			await _dataProvider.SaveSettingsAsync("Championship", championship);
			await _dataProvider.SaveSettingsAsync("Language", language);
			await _dataProvider.SaveSettingsAsync("WindowSize", windowSize);
		}

		private void InitialSettingsWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SaveSettings_Click(sender, e);
			}
			else if (e.Key == Key.Escape)
			{
				this.DialogResult = false;
				this.Close();
			}
		}
	}
}