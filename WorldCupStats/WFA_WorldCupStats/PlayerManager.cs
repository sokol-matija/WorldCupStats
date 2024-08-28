using DataLayer;
using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public class PlayerManager
	{
		private readonly IDataProvider _dataProvider;
		public List<Player> AllPlayers { get; private set; }
		public List<PlayerControl> FavoritePlayers { get; private set; }
		public List<PlayerControl> SelectedPlayers { get; private set; }
		private string CurrentTeamFifaCode { get; set; }

		public PlayerManager(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			AllPlayers = new List<Player>();
			FavoritePlayers = new List<PlayerControl>();
			SelectedPlayers = new List<PlayerControl>();
		}

		public async Task LoadPlayersAsync(string fifaCode, string championship)
		{
			CurrentTeamFifaCode = fifaCode;
			AllPlayers = await _dataProvider.GetPlayersByTeamAsync(fifaCode, championship.ToLower());
		}

		public async Task LoadFavoritePlayersAsync(string fifaCode)
		{
			var favoritePlayerNames = await _dataProvider.LoadFavoritePlayersAsync(fifaCode);
			FavoritePlayers = AllPlayers
				.Where(p => favoritePlayerNames.Contains(p.Name))
				.Select(p => new PlayerControl(p) { IsFavorite = true })
				.ToList();
		}

		public async Task ToggleFavoritePlayerAsync(PlayerControl playerControl)
		{
			if (playerControl.IsFavorite)
			{
				if (FavoritePlayers.Count >= 3)
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
					if (!FavoritePlayers.Any(fp => fp.Player.Name == playerControl.Player.Name))
					{
						playerControl.IsFavorite = true;
						FavoritePlayers.Add(playerControl);
					}
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
			if (!string.IsNullOrEmpty(CurrentTeamFifaCode))
			{
				var favoritePlayerNames = FavoritePlayers.Select(pc => pc.Player.Name).ToList();
				await _dataProvider.SaveFavoritePlayersAsync(CurrentTeamFifaCode, favoritePlayerNames);
			}
		}
	}
}