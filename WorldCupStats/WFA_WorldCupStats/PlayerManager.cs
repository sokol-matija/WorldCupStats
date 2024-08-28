using DataLayer;
using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public class PlayerManager
	{
		private readonly LogForm _logForm;
		private readonly IDataProvider _dataProvider;
		private readonly SettingsManager _settingsManager;

		public List<Player> AllPlayers { get; private set; }
		public HashSet<PlayerControl> FavoritePlayers { get; private set; }
		public List<PlayerControl> SelectedPlayers { get; private set; }
		private string CurrentTeamFifaCode { get; set; }

		public PlayerManager(IDataProvider dataProvider, SettingsManager settingsManager, LogForm logForm)
		{
			_logForm = logForm;
			_settingsManager = settingsManager;
			_dataProvider = dataProvider;
			AllPlayers = new List<Player>();
			FavoritePlayers = new HashSet<PlayerControl>(new PlayerControlComparer());
			SelectedPlayers = new List<PlayerControl>();
		}

		public async Task LoadPlayersAsync(string fifaCode, string championship)
		{
			CurrentTeamFifaCode = fifaCode;
			AllPlayers = await _dataProvider.GetPlayersByTeamAsync(fifaCode, championship.ToLower());
		}

		public async Task LoadFavoritePlayersAsync()
		{
			var favoritePlayerNames = await _settingsManager.LoadFavoritePlayersAsync();

			FavoritePlayers = new HashSet<PlayerControl>(
				AllPlayers
					.Where(p => favoritePlayerNames.Contains(p.Name))
					.Select(p => new PlayerControl(p, _logForm) { IsFavorite = true }),
				new PlayerControlComparer()
			);

			_logForm.Log($"Loaded {FavoritePlayers.Count} favorite players");
		}

		public async Task ToggleFavoritePlayerAsync(PlayerControl playerControl)
		{
			if (playerControl.IsFavorite)
			{
				if (FavoritePlayers.Count >= 4)
				{
					MessageBox.Show("You can only have up to 3 favorite players.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
					playerControl.IsFavorite = false;
					return;
				}
				FavoritePlayers.Add(playerControl);
			}
			else
			{
				FavoritePlayers.Remove(playerControl);
			}

			await SaveFavoritePlayersAsync();
			OnFavoritePlayersChanged();
		}

		public event EventHandler FavoritePlayersChanged;

		protected virtual void OnFavoritePlayersChanged()
		{
			FavoritePlayersChanged?.Invoke(this, EventArgs.Empty);
		}

		public void ToggleSelectPlayer(PlayerControl playerControl)
		{
			if (playerControl.IsSelected)
			{
				if (!SelectedPlayers.Contains(playerControl))
				{
					SelectedPlayers.Add(playerControl);
				}
			}
			else
			{
				SelectedPlayers.Remove(playerControl);
			}
		}

		public void MoveSelectedPlayersToFavorites()
		{
			foreach (var playerControl in SelectedPlayers.ToList())
			{
				if (FavoritePlayers.Count < 3)
				{
					playerControl.IsFavorite = true;
					FavoritePlayers.Add(playerControl);
				}
				else
				{
					MessageBox.Show("You can only have up to 3 favorite players.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
			}

			SelectedPlayers.Clear();
			SaveFavoritePlayersAsync();
		}

		private async Task SaveFavoritePlayersAsync()
		{
			var favoritePlayerNames = FavoritePlayers.Select(pc => pc.Player.Name).Distinct().ToList();
			await _settingsManager.SaveFavoritePlayersAsync(favoritePlayerNames);
		}

		public void ResetPlayers()
		{
			AllPlayers.Clear();
			FavoritePlayers.Clear();
			SelectedPlayers.Clear();
			OnFavoritePlayersChanged();
		}
	}
}