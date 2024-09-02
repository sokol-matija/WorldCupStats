using System.Windows;

namespace WPF_WorldCupStats
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ShowInitialSettingsIfNeeded();
		}

		private void ShowInitialSettingsIfNeeded()
		{
			// TODO: Implementirati provjeru postojanja postavki
			bool settingsExist = false; // Zamijeniti s stvarnom provjerom

			if (!settingsExist)
			{
				var settingsWindow = new InitialSettingsWindow();
				if (settingsWindow.ShowDialog() == true)
				{
					// Postavke su spremljene, možemo nastaviti s inicijalizacijom glavnog prozora
					InitializeMainWindow();
				}
				else
				{
					// Korisnik je odustao od postavljanja postavki, zatvaramo aplikaciju
					Application.Current.Shutdown();
				}
			}
			else
			{
				// Postavke već postoje, nastavljamo s inicijalizacijom glavnog prozora
				InitializeMainWindow();
			}
		}

		private void InitializeMainWindow()
		{
			// TODO: Implementirati učitavanje postavki i inicijalizaciju glavnog prozora
		}
	}
}