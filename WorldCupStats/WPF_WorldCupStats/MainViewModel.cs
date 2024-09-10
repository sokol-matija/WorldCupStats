using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_WorldCupStats
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private readonly IDataProvider _dataProvider;
		private string _selectedChampionship;

		public ObservableCollection<Team> Teams { get; set; }
		public ObservableCollection<Match> Matches { get; set; }

		private Team _selectedTeam;
		public Team SelectedTeam
		{
			get => _selectedTeam;
			set
			{
				if (_selectedTeam != value)
				{
					_selectedTeam = value;
					OnPropertyChanged();
					LoadTeamPlayersAsync();
					UpdateOpponentTeams();
				}
			}
		}

		private Team _selectedOpponentTeam;
		public Team SelectedOpponentTeam
		{
			get => _selectedOpponentTeam;
			set
			{
				if (_selectedOpponentTeam != value)
				{
					_selectedOpponentTeam = value;
					OnPropertyChanged();
					UpdateMatchResult();
				}
			}
		}

		private string _matchResult;
		public string MatchResult
		{
			get => _matchResult;
			set
			{
				if (_matchResult != value)
				{
					_matchResult = value;
					OnPropertyChanged();
				}
			}
		}

		public ObservableCollection<Player> TeamPlayers { get; set; }
		public ObservableCollection<Team> OpponentTeams { get; set; }
		public ObservableCollection<Player> FavoritePlayers { get; set; }

		private bool _isLoading;
		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					OnPropertyChanged();
				}
			}
		}

		public MainViewModel(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			Teams = new ObservableCollection<Team>();
			Matches = new ObservableCollection<Match>();
			TeamPlayers = new ObservableCollection<Player>();
			OpponentTeams = new ObservableCollection<Team>();
			FavoritePlayers = new ObservableCollection<Player>();
		}

		public async Task LoadDataAsync()
		{
			try
			{
				IsLoading = true;
				System.Diagnostics.Debug.WriteLine("Starting LoadDataAsync");

				_selectedChampionship = await _dataProvider.LoadSettingsAsync("Championship");
				System.Diagnostics.Debug.WriteLine($"Loaded Championship: {_selectedChampionship}");

				var teams = await _dataProvider.GetTeamsAsync(_selectedChampionship);
				System.Diagnostics.Debug.WriteLine($"Loaded {teams.Count} teams");

				var matches = await _dataProvider.GetMatchesAsync(_selectedChampionship);
				System.Diagnostics.Debug.WriteLine($"Loaded {matches.Count} matches");

				Teams.Clear();
				foreach (var team in teams)
				{
					Teams.Add(team);
					System.Diagnostics.Debug.WriteLine($"Added team: {team.Country}, FIFA Code: {team.FifaCode}");
				}

				Matches.Clear();
				foreach (var match in matches)
				{
					Matches.Add(match);
					System.Diagnostics.Debug.WriteLine($"Added match: {match.HomeTeam.Country} vs {match.AwayTeam.Country}");
				}

				UpdateTeamStatistics();
				System.Diagnostics.Debug.WriteLine("Updated team statistics");

				if (Teams.Any())
				{
					DebugTeamData(Teams.First());
				}

				string favoriteTeam = await _dataProvider.LoadFavoriteTeamAsync();
				System.Diagnostics.Debug.WriteLine($"Loaded favorite team: {favoriteTeam}");

				if (!string.IsNullOrEmpty(favoriteTeam))
				{
					SelectedTeam = Teams.FirstOrDefault(t => t.FifaCode == favoriteTeam);
					if (SelectedTeam != null)
					{
						System.Diagnostics.Debug.WriteLine($"Set selected team to: {SelectedTeam.Country}");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Favorite team not found in loaded teams");
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("No favorite team set");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in LoadDataAsync: {ex.Message}");
				System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
				MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				IsLoading = false;
				System.Diagnostics.Debug.WriteLine("Finished LoadDataAsync");
			}
		}

		public async Task LoadTeamPlayersAsync()
		{
			if (SelectedTeam != null)
			{
				var players = await _dataProvider.GetPlayersByTeamAsync(SelectedTeam.FifaCode, _selectedChampionship);
				TeamPlayers.Clear();
				foreach (var player in players)
				{
					TeamPlayers.Add(player);
				}
				await LoadFavoritePlayers();
			}
		}

		private void UpdateOpponentTeams()
		{
			if (SelectedTeam != null)
			{
				OpponentTeams.Clear();
				foreach (var match in Matches)
				{
					if (match.HomeTeam.Code == SelectedTeam.FifaCode)
					{
						OpponentTeams.Add(Teams.FirstOrDefault(t => t.FifaCode == match.AwayTeam.Code));
					}
					else if (match.AwayTeam.Code == SelectedTeam.FifaCode)
					{
						OpponentTeams.Add(Teams.FirstOrDefault(t => t.FifaCode == match.HomeTeam.Code));
					}
				}
				OnPropertyChanged(nameof(OpponentTeams));
			}
		}

		private void UpdateMatchResult()
		{
			if (SelectedTeam != null && SelectedOpponentTeam != null)
			{
				var match = Matches.FirstOrDefault(m =>
					(m.HomeTeam.Code == SelectedTeam.FifaCode && m.AwayTeam.Code == SelectedOpponentTeam.FifaCode) ||
					(m.AwayTeam.Code == SelectedTeam.FifaCode && m.HomeTeam.Code == SelectedOpponentTeam.FifaCode));

				if (match != null)
				{
					MatchResult = $"{match.HomeTeam.Goals} : {match.AwayTeam.Goals}";
				}
				else
				{
					MatchResult = "N/A";
				}
			}
			else
			{
				MatchResult = string.Empty;
			}
		}

		public async Task AddToFavorites(Player player)
		{
			if (!FavoritePlayers.Contains(player) && FavoritePlayers.Count < 3)
			{
				FavoritePlayers.Add(player);
				await SaveFavoritePlayers();
			}
		}

		public async Task RemoveFromFavorites(Player player)
		{
			if (FavoritePlayers.Contains(player))
			{
				FavoritePlayers.Remove(player);
				await SaveFavoritePlayers();
			}
		}

		private async Task SaveFavoritePlayers()
		{
			var favoritePlayerNames = FavoritePlayers.Select(p => p.Name).ToList();
			await _dataProvider.SaveFavoritePlayersAsync(_selectedChampionship, SelectedTeam.FifaCode, favoritePlayerNames);
		}

		private async Task LoadFavoritePlayers()
		{
			if (SelectedTeam != null)
			{
				var favoritePlayerNames = await _dataProvider.LoadFavoritePlayersAsync(_selectedChampionship, SelectedTeam.FifaCode);
				FavoritePlayers.Clear();
				foreach (var playerName in favoritePlayerNames)
				{
					var player = TeamPlayers.FirstOrDefault(p => p.Name == playerName);
					if (player != null)
					{
						FavoritePlayers.Add(player);
					}
				}
			}
		}
		
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public List<PlayerPosition> GeneratePositions(string formation)
		{
			// Ovo je pojednostavljeni primjer za formaciju 4-4-2
			return new List<PlayerPosition>
			{
				new PlayerPosition { X = 0.5, Y = 0.95, Position = "Goalie" },
				new PlayerPosition { X = 0.2, Y = 0.75, Position = "Defender" },
				new PlayerPosition { X = 0.4, Y = 0.75, Position = "Defender" },
				new PlayerPosition { X = 0.6, Y = 0.75, Position = "Defender" },
				new PlayerPosition { X = 0.8, Y = 0.75, Position = "Defender" },
				new PlayerPosition { X = 0.2, Y = 0.5, Position = "Midfield" },
				new PlayerPosition { X = 0.4, Y = 0.5, Position = "Midfield" },
				new PlayerPosition { X = 0.6, Y = 0.5, Position = "Midfield" },
				new PlayerPosition { X = 0.8, Y = 0.5, Position = "Midfield" },
				new PlayerPosition { X = 0.35, Y = 0.25, Position = "Forward" },
				new PlayerPosition { X = 0.65, Y = 0.25, Position = "Forward" }
			};
		}
		private void UpdateTeamStatistics()
		{
			foreach (var match in Matches)
			{
				UpdateTeamStatsFromMatch(match.HomeTeam, match.AwayTeam, match);
				UpdateTeamStatsFromMatch(match.AwayTeam, match.HomeTeam, match);
			}
		}

		private void UpdateTeamStatsFromMatch(TeamResult teamResult, TeamResult opponentResult, Match match)
		{
			var team = Teams.FirstOrDefault(t => t.FifaCode == teamResult.Code);
			if (team != null)
			{
				team.GamesPlayed++;
				team.GoalsFor += teamResult.Goals ?? 0;
				team.GoalsAgainst += opponentResult.Goals ?? 0;

				if (teamResult.Goals > opponentResult.Goals)
				{
					team.Wins++;
				}
				else if (teamResult.Goals < opponentResult.Goals)
				{
					team.Losses++;
				}
				else
				{
					team.Draws++;
				}

				team.GoalDifferential = team.GoalsFor - team.GoalsAgainst;
				team.Points = (team.Wins * 3) + team.Draws;
			}
		}



	}
}