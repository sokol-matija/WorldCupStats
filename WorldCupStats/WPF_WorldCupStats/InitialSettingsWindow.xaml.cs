using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_WorldCupStats
{
	public partial class InitialSettingsWindow : Window
	{
		public InitialSettingsWindow()
		{
			InitializeComponent();
		}

		private void SaveSettings_Click(object sender, RoutedEventArgs e)
		{
			// Implementirati logiku spremanja postavki
			SaveSettings();
			this.DialogResult = true;
			this.Close();
		}

		private void SaveSettings()
		{
			// Implementirati spremanje postavki u tekstualnu datoteku
			// Koristiti vrijednosti iz ComboBox kontrola
			string championship = (cbChampionship.SelectedItem as ComboBoxItem)?.Content.ToString();
			string language = (cbLanguage.SelectedItem as ComboBoxItem)?.Content.ToString();
			string windowSize = (cbWindowSize.SelectedItem as ComboBoxItem)?.Content.ToString();

			// TODO: Spremiti ove vrijednosti u tekstualnu datoteku
		}
	}
}