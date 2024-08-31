using DataLayer.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFA_WorldCupStats.Managers;

namespace WFA_WorldCupStats
{
    public class EventHandlers
	{
		private readonly MainForm _form;
		private readonly SettingsManager _settingsManager;
		private readonly PlayerManager _playerManager;
		private readonly UIManager _uiManager;

		public EventHandlers(MainForm form, SettingsManager settingsManager, PlayerManager playerManager, UIManager uiManager)
		{
			_form = form;
			_settingsManager = settingsManager;
			_playerManager = playerManager;
			_uiManager = uiManager;
		}

		public async Task HandleTeamSelectionChanged(object sender, EventArgs e)
		{
			if (_form.cmbTeams.SelectedItem is Team selectedTeam)
			{
				_form._logForm.Log($"Team changed to: {selectedTeam.Country} ({selectedTeam.FifaCode})");
				await _settingsManager.SaveFavoriteTeamAsync(selectedTeam.FifaCode);
				await LoadPlayerDetailsAsync(selectedTeam.FifaCode);
			}
		}

		public void HandleMoveToFavoritesClick(object sender, EventArgs e)
		{
			_playerManager.MoveSelectedPlayersToFavorites();
			_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
			_uiManager.UpdateMoveToFavoritesButtonVisibility(_playerManager.SelectedPlayers.Count);
		}

		public void HandleSettingsClick(object sender, EventArgs e)
		{
			OpenSettingsAsync();
		}

		public void HandlePrintStatisticsClick(object sender, EventArgs e)
		{
			string report = _uiManager.GenerateStatisticsReport();
			_uiManager.ShowPrintPreview(report);
		}

		public async Task HandlePlayerFavoriteToggled(object sender, EventArgs e)
		{
			if (sender is PlayerControl playerControl)
			{
				await _playerManager.ToggleFavoritePlayerAsync(playerControl);
				_uiManager.UpdatePlayerPanels(_playerManager.AllPlayers, _playerManager.FavoritePlayers);
			}
		}

		public void HandlePlayerSelectionToggled(object sender, EventArgs e)
		{
			if (sender is PlayerControl playerControl)
			{
				_playerManager.ToggleSelectPlayer(playerControl);
				_uiManager.UpdateMoveToFavoritesButtonVisibility(_playerManager.SelectedPlayers.Count);
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
				_form.HandleException("Error loading player details", ex);
			}
			finally
			{
				_uiManager.ShowLoadingIndicator(false);
			}
		}

		private async void OpenSettingsAsync()
		{
			if (await _settingsManager.ShowSettingsFormAsync())
			{
				await _form.ApplySettingsAsync();
			}
		}
	}
}