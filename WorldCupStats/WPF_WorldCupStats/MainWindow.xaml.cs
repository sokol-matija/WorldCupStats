using DataLayer;
using DataLayer.Models;
using System.Diagnostics;
using System.Windows;

namespace WPF_WorldCupStats
{
	public partial class MainWindow : Window
	{
		private readonly MainViewModel _viewModel;
		private readonly IDataProvider _dataProvider;

		public MainWindow()
		{
			InitializeComponent();
			_dataProvider = DataProviderFactory.CreateDataProvider();
			_viewModel = new MainViewModel(_dataProvider);
			DataContext = _viewModel;
			Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await ShowInitialSettingsIfNeeded();
			await _viewModel.LoadDataAsync();
		}

		private async Task ShowInitialSettingsIfNeeded()
		{
			try
			{
				Debug.WriteLine("ShowInitialSettingsIfNeeded started");
				string championship = await _dataProvider.LoadSettingsAsync("Championship");
				string language = await _dataProvider.LoadSettingsAsync("Language");
				string windowSize = await _dataProvider.LoadSettingsAsync("WindowSize");

				if (string.IsNullOrEmpty(championship) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(windowSize))
				{
					var settingsWindow = new InitialSettingsWindow(_dataProvider);
					if (settingsWindow.ShowDialog() == true)
					{
						ApplySettings();
					}
					else
					{
						Application.Current.Shutdown();
					}
				}
				else
				{
					ApplySettings();
				}
				Debug.WriteLine("ShowInitialSettingsIfNeeded completed");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error in ShowInitialSettingsIfNeeded: {ex.Message}");
				throw;
			}
		}

		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			OpenSettingsWindow();
		}

		private void OpenStatistics_Click(object sender, RoutedEventArgs e)
		{
			OpenStatisticsWindow();
		}

		private void OpenSettingsWindow()
		{
			var settingsWindow = new InitialSettingsWindow(_dataProvider);
			if (settingsWindow.ShowDialog() == true)
			{
				ApplySettings();
			}
		}

		private void OpenStatisticsWindow()
		{
			var statisticsWindow = new StatisticsWindow(_viewModel);
			statisticsWindow.Show();
		}

		private async void ApplySettings()
		{
			try
			{
				Debug.WriteLine("ApplySettings started");

				// Učitaj postavke
				string championship = await _dataProvider.LoadSettingsAsync("Championship");
				string language = await _dataProvider.LoadSettingsAsync("Language");
				string windowSize = await _dataProvider.LoadSettingsAsync("WindowSize");

				// Primijeni jezik
				if (!string.IsNullOrEmpty(language))
				{
					ChangeLanguage(language);
				}

				// Primijeni veličinu prozora
				if (!string.IsNullOrEmpty(windowSize))
				{
					ApplyWindowSize(windowSize);
				}

				// Primijeni prvenstvo i osvježi podatke
				if (!string.IsNullOrEmpty(championship))
				{
					//await _viewModel.LoadChampionshipDataAsync(championship);
				}

				// Osvježi bindings
				this.UpdateLayout();

				Debug.WriteLine("ApplySettings completed");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error in ApplySettings: {ex.Message}");
				MessageBox.Show($"An error occurred while applying settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ChangeLanguage(string languageCode)
		{
			try
			{
				Debug.WriteLine($"Changing language to: {languageCode}");
				LocalizationManager.SetLanguage(languageCode);

				ResourceDictionary dict = new ResourceDictionary();
				string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				string resourcePath;

				switch (languageCode.ToLower())
				{
					case "hr":
						resourcePath = $"pack://application:,,,/{assemblyName};component/Resources/Lang.hr.xaml";
						break;
					case "en":
					default:
						resourcePath = $"pack://application:,,,/{assemblyName};component/Resources/Lang.en.xaml";
						break;
				}

				Debug.WriteLine($"Attempting to load resource from: {resourcePath}");
				dict.Source = new Uri(resourcePath, UriKind.Absolute);

				// Ukloni sve postojeće rječnike
				Application.Current.Resources.MergedDictionaries.Clear();
				// Dodaj novi rječnik
				Application.Current.Resources.MergedDictionaries.Add(dict);

				Debug.WriteLine("Language change completed successfully");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error changing language: {ex.Message}");
				MessageBox.Show($"An error occurred while changing the language: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ApplyWindowSize(string windowSize)
		{
			switch (windowSize.ToLower())
			{
				case "fullscreen":
					WindowState = WindowState.Maximized;
					break;
				case "1920x1080":
					Width = 1920;
					Height = 1080;
					WindowState = WindowState.Normal;
					break;
				case "1280x720":
					Width = 1280;
					Height = 720;
					WindowState = WindowState.Normal;
					break;
				case "800x600":
					Width = 800;
					Height = 600;
					WindowState = WindowState.Normal;
					break;
				default:
					Debug.WriteLine($"Unknown window size: {windowSize}");
					break;
			}
		}


		private void btnFavoriteTeamInfo_Click(object sender, RoutedEventArgs e)
		{
			ShowTeamInfo(_viewModel.SelectedTeam);
		}

		private void btnOpponentTeamInfo_Click(object sender, RoutedEventArgs e)
		{
			ShowTeamInfo(_viewModel.SelectedOpponentTeam);
		}

		private void ShowTeamInfo(Team team)
		{
			if (team != null)
			{
				var teamInfoWindow = new TeamInfoWindow(team);
				teamInfoWindow.Show();
			}
		}

		private void PlayersList_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(Player)))
			{
				Player player = e.Data.GetData(typeof(Player)) as Player;
				_viewModel.RemoveFromFavorites(player);
			}
		}

		private void FavoritesList_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(Player)))
			{
				Player player = e.Data.GetData(typeof(Player)) as Player;
				_viewModel.AddToFavorites(player);
			}
		}

		private void PrintStatistics_Click(object sender, RoutedEventArgs e)
		{
			//PrintService printService = new PrintService();
			//printService.PrintStatistics(_viewModel);
		}


		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			var result = MessageBox.Show("Are you sure you want to close the application?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
			base.OnClosing(e);
		}
	}
}