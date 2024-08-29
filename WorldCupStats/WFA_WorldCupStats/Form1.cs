using DataLayer;
using DataLayer.Models;
using System;

namespace WFA_WorldCupStats
{
	public partial class Form1 : Form
	{
		private readonly IDataProvider _dataProvider;
		private readonly SettingsManager _settingsManager;
		private readonly PlayerManager _playerManager;
		private readonly UIManager _uiManager;
		public readonly LogForm _logForm;
		private readonly DragDropManager _dragDropManager;
		private readonly EventHandlers _eventHandlers;

		public Form1()
		{
			InitializeComponent();
			_dataProvider = DataProviderFactory.CreateDataProvider();
			_logForm = new LogForm();
			_settingsManager = new SettingsManager(_dataProvider, _logForm);
			_playerManager = new PlayerManager(_dataProvider, _settingsManager, _logForm);
			_uiManager = new UIManager(this, _logForm);
			_dragDropManager = new DragDropManager(this, _playerManager, _uiManager, _logForm);
			_eventHandlers = new EventHandlers(this, _settingsManager, _playerManager, _uiManager);

			_logForm.Show();

			InitializeAsync();
		}

		private async void InitializeAsync()
		{
			try
			{
				await _settingsManager.LoadInitialSettingsAsync();
				if (!await _settingsManager.ShowInitialSettingsFormAsync())
				{
					Application.Exit();
					return;
				}
				await ApplySettingsAsync();
			}
			catch (Exception ex)
			{
				HandleException("Initialization error", ex);
			}
		}

		public async Task ApplySettingsAsync()
		{
			_uiManager.ChangeLanguage(_settingsManager.SelectedLanguage);
			await LoadChampionshipDataAsync();
			_uiManager.ApplyLocalization();

			_playerManager.ResetPlayers();
			if (!string.IsNullOrEmpty(_settingsManager.FavoriteTeam))
			{
				await LoadPlayerDetailsAsync(_settingsManager.FavoriteTeam);
			}
		}

		private async Task LoadChampionshipDataAsync()
		{
			try
			{
				_uiManager.ShowLoadingIndicator(true);
				await LoadTeamsAndMatchesAsync();
				await LoadAndDisplayStatisticsAsync();
			}
			catch (Exception ex)
			{
				HandleException("Error loading championship data", ex);
			}
			finally
			{
				_uiManager.ShowLoadingIndicator(false);
			}
		}

		private async Task LoadTeamsAndMatchesAsync()
		{
			var teams = await _dataProvider.GetTeamsAsync(_settingsManager.SelectedChampionship.ToLower());
			_uiManager.PopulateTeamsComboBox(teams, _settingsManager.FavoriteTeam);

			var matches = await _dataProvider.GetMatchesAsync(_settingsManager.SelectedChampionship.ToLower());
			_uiManager.UpdateMatchesList(matches);
		}

		private async Task LoadAndDisplayStatisticsAsync()
		{
			var topScorers = await _dataProvider.GetTopScorersAsync(_settingsManager.SelectedChampionship.ToLower(), 5);
			var yellowCards = await _dataProvider.GetYellowCardsAsync(_settingsManager.SelectedChampionship.ToLower(), 5);

			_uiManager.UpdateStatisticsList(lstTopScorers, topScorers);
			_uiManager.UpdateStatisticsList(lstYellowCards, yellowCards);
		}

		private async Task LoadPlayerDetailsAsync(string fifaCode)
		{
			try
			{
				_uiManager.ShowLoadingIndicator(true);
				await _playerManager.LoadPlayersAsync(fifaCode, _settingsManager.SelectedChampionship);
				await _playerManager.LoadFavoritePlayersAsync();
				_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
			}
			catch (Exception ex)
			{
				HandleException("Error loading player details", ex);
			}
			finally
			{
				_uiManager.ShowLoadingIndicator(false);
			}
		}

		public void HandleException(string message, Exception ex)
		{
			_logForm.Log($"{message}: {ex.Message}");
			MessageBox.Show($"{message}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		// Event handlers
		private void cmbTeams_SelectedIndexChanged(object sender, EventArgs e) => _eventHandlers.HandleTeamSelectionChanged(sender, e);
		private void btnMoveToFavorites_Click(object sender, EventArgs e) => _eventHandlers.HandleMoveToFavoritesClick(sender, e);
		private void mnuSettings_Click(object sender, EventArgs e) => _eventHandlers.HandleSettingsClick(sender, e);
		private void printStatistics_Click(object sender, EventArgs e) => _eventHandlers.HandlePrintStatisticsClick(sender, e);
		public void PlayerControl_FavoriteToggled(object sender, EventArgs e) => _eventHandlers.HandlePlayerFavoriteToggled(sender, e);
		public void PlayerControl_SelectionToggled(object sender, EventArgs e) => _eventHandlers.HandlePlayerSelectionToggled(sender, e);
	}
}