using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public class DragDropManager
	{
		private readonly Form1 _form;
		private readonly PlayerManager _playerManager;
		private readonly UIManager _uiManager;
		private readonly LogForm _logForm;

		public DragDropManager(Form1 form, PlayerManager playerManager, UIManager uiManager, LogForm logForm)
		{
			_form = form;
			_playerManager = playerManager;
			_uiManager = uiManager;
			_logForm = logForm;
			InitializeDragAndDrop();
		}

		private void InitializeDragAndDrop()
		{
			_form.pnlAllPlayers.AllowDrop = true;
			_form.pnlFavoritePlayers.AllowDrop = true;

			_form.pnlAllPlayers.DragEnter += Panel_DragEnter;
			_form.pnlAllPlayers.DragDrop += PnlAllPlayers_DragDrop;
			_form.pnlFavoritePlayers.DragEnter += Panel_DragEnter;
			_form.pnlFavoritePlayers.DragDrop += PnlFavoritePlayers_DragDrop;
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
				_form.pnlFavoritePlayers.Controls.Remove(playerControl);
				_form.pnlAllPlayers.Controls.Add(playerControl);
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
				_form.pnlAllPlayers.Controls.Remove(playerControl);
				_form.pnlFavoritePlayers.Controls.Add(playerControl);
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
	}
}