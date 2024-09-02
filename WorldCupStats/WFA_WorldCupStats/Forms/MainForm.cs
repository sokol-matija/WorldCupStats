using DataLayer;
using DataLayer.Models;
using System;
using WFA_WorldCupStats.Controls;
using WFA_WorldCupStats.Managers;

namespace WFA_WorldCupStats
{
    public partial class MainForm : Form
	{
		private readonly IDataProvider _dataProvider;
		private readonly SettingsManager _settingsManager;
		private readonly PlayerManager _playerManager;
		private readonly UIManager _uiManager;
		public readonly LogForm _logForm;
		private readonly DragDropManager _dragDropManager;
		private readonly EventHandlers _eventHandlers;

		public MainForm()
		{
			InitializeComponent();
			_dataProvider = DataProviderFactory.CreateDataProvider();
			_logForm = new LogForm();
			_settingsManager = new SettingsManager(_dataProvider, _logForm);
			_playerManager = new PlayerManager(_dataProvider, _settingsManager, _logForm);
			_uiManager = new UIManager(this, _logForm, _dataProvider);
			_dragDropManager = new DragDropManager(this, _playerManager, _uiManager, _logForm);
			_eventHandlers = new EventHandlers(this, _settingsManager, _playerManager, _uiManager);

			mnuPrintStatistics.Click += mnuPrintStatistics_Click;
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
			//_uiManager.UpdateRankingList(matches);
			//TODO: Implement UpdateRankingList method
		}

		private async Task LoadAndDisplayStatisticsAsync()
		{
			if (string.IsNullOrEmpty(_settingsManager.FavoriteTeam))
			{
				// O�isti panele ako nema odabranog tima
				_uiManager.ClearStatisticsPanels();
				_logForm.Log("No favorite team selected. Clearing panels.");
				return;
			}

			_logForm.Log($"Loading statistics for team: {_settingsManager.FavoriteTeam}");

			var topScorers = await _dataProvider.GetTopScorersAsync(_settingsManager.SelectedChampionship.ToLower(), _settingsManager.FavoriteTeam);
			_logForm.Log($"Top Scorers retrieved: {topScorers.Count}");

			var yellowCards = await _dataProvider.GetYellowCardsAsync(_settingsManager.SelectedChampionship.ToLower(), _settingsManager.FavoriteTeam);
			_logForm.Log($"Yellow Cards retrieved: {yellowCards.Count}");

			var matches = await _dataProvider.GetMatchesByAttendanceAsync(_settingsManager.SelectedChampionship.ToLower(), _settingsManager.FavoriteTeam);
			_logForm.Log($"Matches retrieved: {matches.Count}");

			_logForm.Log("Updating Top Scorers panel");
			_uiManager.UpdateRankingList(pnlTopScorers, topScorers, (stats) => new RankingPlayerControl(stats, _dataProvider));

			_logForm.Log("Updating Yellow Cards panel");
			_uiManager.UpdateRankingList(pnlYellowCards, yellowCards, (stats) => new RankingPlayerControl(stats, _dataProvider));

			_logForm.Log("Updating Matches panel");
			_uiManager.UpdateRankingList(pnlMatches, matches, (match) => new MatchControl(match));

			_logForm.Log("Finished updating all panels");
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
		private async void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			await _eventHandlers.HandleTeamSelectionChanged(sender, e);
			await LoadAndDisplayStatisticsAsync();

		}
		private void btnMoveToFavorites_Click(object sender, EventArgs e) => _eventHandlers.HandleMoveToFavoritesClick(sender, e);
		private void mnuSettings_Click(object sender, EventArgs e) => _eventHandlers.HandleSettingsClick(sender, e);
		private void printStatistics_Click(object sender, EventArgs e) => _eventHandlers.HandlePrintStatisticsClick(sender, e);
		public void PlayerControl_FavoriteToggled(object sender, EventArgs e) => _eventHandlers.HandlePlayerFavoriteToggled(sender, e);
		public void PlayerControl_SelectionToggled(object sender, EventArgs e) => _eventHandlers.HandlePlayerSelectionToggled(sender, e);
		private void mnuPrintStatistics_Click(object sender, EventArgs e) => _uiManager.PrepareAndShowPrintPreview();

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				var result = MessageBox.Show(
					Strings.ApplicationClosingMessage,
					Strings.ApplicatonClosingTitle,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);

				if (result == DialogResult.No)
				{
					e.Cancel = true;
				}
			}
			base.OnFormClosing(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				// Potvrdi zatvaranje
				Close();
				return true;
			}
			else if (keyData == Keys.Escape)
			{
				// Odustani od zatvaranja
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}