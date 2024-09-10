using DataLayer;
using DataLayer.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

			cbFavoriteTeam.ItemsSource = null;
			cbFavoriteTeam.ItemsSource = _viewModel.Teams;
			cbFavoriteTeam.SelectedItem = _viewModel.SelectedTeam;

			DisplayPlayersOnField();
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

				string championship = await _dataProvider.LoadSettingsAsync("Championship");
				string language = await _dataProvider.LoadSettingsAsync("Language");
				string windowSize = await _dataProvider.LoadSettingsAsync("WindowSize");

				if (!string.IsNullOrEmpty(language))
				{
					ChangeLanguage(language);
				}

				if (!string.IsNullOrEmpty(windowSize))
				{
					ApplyWindowSize(windowSize);
				}

				if (!string.IsNullOrEmpty(championship))
				{
					await _viewModel.LoadDataAsync();
				}

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
				string resourcePath = languageCode.ToLower() == "hr"
					? $"pack://application:,,,/{assemblyName};component/Resources/Lang.hr.xaml"
					: $"pack://application:,,,/{assemblyName};component/Resources/Lang.en.xaml";

				Debug.WriteLine($"Attempting to load resource from: {resourcePath}");
				dict.Source = new Uri(resourcePath, UriKind.Absolute);

				Application.Current.Resources.MergedDictionaries.Clear();
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

		private void DisplayPlayersOnField()
		{
			gridFormation.Children.Clear();
			var positions = _viewModel.GeneratePositions("4-4-2"); // Možete promijeniti formaciju po potrebi

			for (int i = 0; i < Math.Min(positions.Count, _viewModel.TeamPlayers.Count); i++)
			{
				var player = _viewModel.TeamPlayers[i];
				var position = positions[i];
				
				var playerControl = new PlayerControl();
				
				playerControl.SetPlayerInfo(player.Name, player.ShirtNumber.ToString(), "C:\\Temp\\profile.png");

				Grid.SetColumn(playerControl, 0);
				Grid.SetRow(playerControl, 0);

				var left = position.X * gridFormation.ActualWidth - (playerControl.Width / 2);
				var top = position.Y * gridFormation.ActualHeight - (playerControl.Height / 2);

				Canvas.SetLeft(playerControl, left);
				Canvas.SetTop(playerControl, top);

				gridFormation.Children.Add(playerControl);
			}
		}

		private async void cbFavoriteTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			await _viewModel.LoadTeamPlayersAsync();
			DisplayPlayersOnField();
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

		private void OnExitClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}