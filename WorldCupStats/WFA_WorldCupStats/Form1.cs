using DataLayer;
using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public partial class Form1 : Form
	{
		private readonly IDataProvider _dataProvider;
		private readonly SettingsManager _settingsManager;
		private readonly PlayerManager _playerManager;
		private readonly UIManager _uiManager;
		public readonly LogForm _logForm;

		public Form1()
		{
			InitializeComponent();
			InitializeDragAndDrop();
			_dataProvider = DataProviderFactory.CreateDataProvider();
			_logForm = new LogForm();
			_settingsManager = new SettingsManager(_dataProvider, _logForm);
			_playerManager = new PlayerManager(_dataProvider, _settingsManager, _logForm);
			_uiManager = new UIManager(this, _logForm);
			_logForm.Show();

			_playerManager.FavoritePlayersChanged += PlayerManager_FavoritePlayersChanged;
			_settingsManager.SettingsChanged += SettingsManager_SettingsChanged;
			InitializeAsync();
		}

		private void InitializeDragAndDrop()
		{
			pnlAllPlayers.AllowDrop = true;
			pnlFavoritePlayers.AllowDrop = true;

			// Add event handlers for drag and drop
			pnlAllPlayers.DragEnter += Panel_DragEnter;
			pnlAllPlayers.DragDrop += PnlAllPlayers_DragDrop;
			pnlFavoritePlayers.DragEnter += Panel_DragEnter;
			pnlFavoritePlayers.DragDrop += PnlFavoritePlayers_DragDrop;
		}

		private void PlayerManager_FavoritePlayersChanged(object sender, EventArgs e)
		{
			_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
		}

		private async void SettingsManager_SettingsChanged(object sender, EventArgs e)
		{
			await ApplySettingsAsync();
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

		private async Task ApplySettingsAsync()
		{
			_uiManager.ChangeLanguage(_settingsManager.SelectedLanguage);
			await LoadChampionshipDataAsync();
			_uiManager.ApplyLocalization();

			// Reset and reload player data
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

		private async void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbTeams.SelectedItem is Team selectedTeam)
			{
				_logForm.Log($"Team changed to: {selectedTeam.Country} ({selectedTeam.FifaCode})");
				await _settingsManager.SaveFavoriteTeamAsync(selectedTeam.FifaCode);
				await LoadPlayerDetailsAsync(selectedTeam.FifaCode);
			}
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

		public async void PlayerControl_FavoriteToggled(object sender, EventArgs e)
		{
			if (sender is PlayerControl playerControl)
			{
				await _playerManager.ToggleFavoritePlayerAsync(playerControl);
				_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
			}
		}

		public void PlayerControl_SelectionToggled(object sender, EventArgs e)
		{
			if (sender is PlayerControl playerControl)
			{
				_playerManager.ToggleSelectPlayer(playerControl);
				_uiManager.UpdateMoveToFavoritesButtonVisibility(_playerManager.SelectedPlayers.Count);
			}
		}

		private void btnMoveToFavorites_Click(object sender, EventArgs e)
		{
			_playerManager.MoveSelectedPlayersToFavorites();
			_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
			_uiManager.UpdateMoveToFavoritesButtonVisibility(_playerManager.SelectedPlayers.Count);
		}

		private void mnuSettings_Click(object sender, EventArgs e)
		{
			OpenSettingsAsync();
		}

		private async void OpenSettingsAsync()
		{
			if (await _settingsManager.ShowSettingsFormAsync())
			{
				await ApplySettingsAsync();
			}
		}

		private void printStatistics_Click(object sender, EventArgs e)
		{
			string report = _uiManager.GenerateStatisticsReport(lstTopScorers, lstYellowCards, lstMatches);
			_uiManager.ShowPrintPreview(report);
		}

		private void Panel_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(PlayerControl)))
			{
				e.Effect = DragDropEffects.Move;
			}
		}

		private void PnlAllPlayers_DragDrop(object sender, DragEventArgs e)
		{
			PlayerControl playerControl = (PlayerControl)e.Data.GetData(typeof(PlayerControl));
			if (playerControl != null && playerControl.IsFavorite)
			{
				MoveToPnlAllPlayers(playerControl);
			}
		}

		private void PnlFavoritePlayers_DragDrop(object sender, DragEventArgs e)
		{
			PlayerControl playerControl = (PlayerControl)e.Data.GetData(typeof(PlayerControl));
			if (playerControl != null && !playerControl.IsFavorite)
			{
				MoveToPnlFavoritePlayers(playerControl);
			}
		}

		private async void MoveToPnlAllPlayers(PlayerControl playerControl)
		{
			if (_playerManager.FavoritePlayers.Contains(playerControl))
			{
				_playerManager.FavoritePlayers.Remove(playerControl);
				pnlFavoritePlayers.Controls.Remove(playerControl);
				pnlAllPlayers.Controls.Add(playerControl);
				playerControl.IsFavorite = false;
				await _playerManager.ToggleFavoritePlayerAsync(playerControl);
				_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
				_logForm.Log($"Player {playerControl.Player.Name} removed from favorites");
			}
		}

		private async void MoveToPnlFavoritePlayers(PlayerControl playerControl)
		{
			if (_playerManager.FavoritePlayers.Count < 3)
			{
				pnlAllPlayers.Controls.Remove(playerControl);
				pnlFavoritePlayers.Controls.Add(playerControl);
				playerControl.IsFavorite = true;
				_playerManager.FavoritePlayers.Add(playerControl);
				await _playerManager.ToggleFavoritePlayerAsync(playerControl);
				_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
				_logForm.Log($"Player {playerControl.Player.Name} added to favorites");
			}
			else
			{
				MessageBox.Show(Strings.FavoritePlayersLimitReached, Strings.LimitReached, MessageBoxButtons.OK, MessageBoxIcon.Information);
				_logForm.Log("Attempted to add more than 3 favorite players");
			}
		}

		private void HandleException(string message, Exception ex)
		{
			_logForm.Log($"{message}: {ex.Message}");
			MessageBox.Show($"{message}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

	}
}