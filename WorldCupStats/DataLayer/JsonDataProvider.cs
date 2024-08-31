using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataLayer.Models;
using Newtonsoft.Json;

namespace DataLayer
{
	public class JsonDataProvider : IDataProvider
	{
		private const string MenAssetsPath = "assets\\men";
		private const string WomenAssetsPath = "assets\\women";
		private const string TeamsFile = "teams.json";
		private const string MatchesFile = "matches.json";
		private const string ResultsFile = "results.json";
		private const string GroupResultsFile = "group_results.json";
		private const string AssetsFolderPath = ".\\assets\\";
		private const string FavoritesFileName = "favorites.json";
		private const string MenFavoritesFileName = "men_favorites.json";
		private const string WomenFavoritesFileName = "women_favorites.json";
		private const string PlayerImagesFolder = "player_images";
		private const string AssetsFolder = "Assets";
		private const string ImagesFolder = "Images";
		private readonly LocalStorageProvider _localStorageProvider;

		public JsonDataProvider()
		{
			_localStorageProvider = new LocalStorageProvider();
			Directory.CreateDirectory(AssetsFolderPath);
		}

		private async Task<T> ReadJsonFile<T>(string gender, string fileName)
		{
			var filePath = Path.Combine("assets", gender, fileName);

			if (!File.Exists(filePath))
			{
				Console.WriteLine($"Current Directory: {Environment.CurrentDirectory}");
				Console.WriteLine($"Attempted file path: {Path.GetFullPath(filePath)}");
				throw new FileNotFoundException($"JSON file not found: {filePath}");
			}

			var json = await File.ReadAllTextAsync(filePath);
			return JsonConvert.DeserializeObject<T>(json);
		}

		public async Task<List<Team>> GetTeamsAsync(string gender)
		{
			return await ReadJsonFile<List<Team>>(gender, TeamsFile);
		}

		public async Task<Team> GetTeamByFifaCodeAsync(string fifaCode, string gender)
		{
			var teams = await GetTeamsAsync(gender);
			return teams.FirstOrDefault(t => t.FifaCode == fifaCode);
		}

		public async Task<List<Match>> GetMatchesAsync(string gender)
		{
			return await ReadJsonFile<List<Match>>(gender, MatchesFile);
		}

		public async Task<List<Match>> GetMatchesByCountryAsync(string fifaCode, string gender)
		{
			var matches = await GetMatchesAsync(gender);
			return matches.Where(m => m.HomeTeam.Code == fifaCode || m.AwayTeam.Code == fifaCode).ToList();
		}

		public async Task<List<GroupResult>> GetGroupResultsAsync(string gender)
		{
			return await ReadJsonFile<List<GroupResult>>(gender, GroupResultsFile);
		}

		public async Task<List<Player>> GetPlayersByTeamAsync(string fifaCode, string gender)
		{
			var matches = await GetMatchesByCountryAsync(fifaCode, gender);
			if (matches.Any())
			{
				var firstMatch = matches.First();
				var teamStatistics = firstMatch.HomeTeam.Code == fifaCode ? firstMatch.HomeTeamStatistics : firstMatch.AwayTeamStatistics;
				return teamStatistics.StartingEleven.Concat(teamStatistics.Substitutes).ToList();
			}
			return new List<Player>();
		}

		public Task SaveSettingsAsync(string key, string value) => _localStorageProvider.SaveSettingsAsync(key, value);
		public Task<string> LoadSettingsAsync(string key) => _localStorageProvider.LoadSettingsAsync(key);
		public Task SaveFavoriteTeamAsync(string fifaCode) => _localStorageProvider.SaveFavoriteTeamAsync(fifaCode);
		public Task<string> LoadFavoriteTeamAsync() => _localStorageProvider.LoadFavoriteTeamAsync();
		public Task SaveFavoritePlayersAsync(List<string> playerNames) => _localStorageProvider.SaveFavoritePlayersAsync(playerNames);
		public Task<List<string>> LoadFavoritePlayersAsync() => _localStorageProvider.LoadFavoritePlayersAsync();

		public async Task<List<PlayerStats>> GetTopScorersAsync(string gender, int count)
		{
			var matches = await GetMatchesAsync(gender);
			var players = new Dictionary<string, int>();

			foreach (var match in matches)
			{
				foreach (var teamEvent in match.HomeTeamEvents.Concat(match.AwayTeamEvents))
				{
					if (teamEvent.TypeOfEvent == "goal" || teamEvent.TypeOfEvent == "goal-penalty")
					{
						if (!players.ContainsKey(teamEvent.Player))
							players[teamEvent.Player] = 0;
						players[teamEvent.Player]++;
					}
				}
			}

			return players.OrderByDescending(p => p.Value)
						  .Take(count)
						  .Select(p => new PlayerStats { Name = p.Key, Count = p.Value })
						  .ToList();
		}

		public async Task<List<PlayerStats>> GetYellowCardsAsync(string gender, int count)
		{
			var matches = await GetMatchesAsync(gender);
			var players = new Dictionary<string, int>();

			foreach (var match in matches)
			{
				foreach (var teamEvent in match.HomeTeamEvents.Concat(match.AwayTeamEvents))
				{
					if (teamEvent.TypeOfEvent == "yellow-card")
					{
						if (!players.ContainsKey(teamEvent.Player))
							players[teamEvent.Player] = 0;
						players[teamEvent.Player]++;
					}
				}
			}

			return players.OrderByDescending(p => p.Value)
						  .Take(count)
						  .Select(p => new PlayerStats { Name = p.Key, Count = p.Value })
						  .ToList();
		}

		public async Task<List<Match>> GetMatchesByAttendanceAsync(string gender, int count)
		{
			var matches = await GetMatchesAsync(gender);
			return matches.OrderByDescending(m => int.Parse(m.Attendance))
						  .Take(count)
						  .ToList();
		}

		public async Task SaveFavoritePlayersAsync(string fifaCode, List<string> playerNames)
		{
			var favoritesFilePath = Path.Combine(AssetsFolderPath, FavoritesFileName);
			var favorites = new Dictionary<string, List<string>>();

			if (File.Exists(favoritesFilePath))
			{
				var json = await File.ReadAllTextAsync(favoritesFilePath);
				favorites = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json) ?? new Dictionary<string, List<string>>();
			}

			favorites[fifaCode] = playerNames;

			var updatedJson = JsonConvert.SerializeObject(favorites);

			Directory.CreateDirectory(AssetsFolderPath);

			await File.WriteAllTextAsync(favoritesFilePath, updatedJson);
		}

		public async Task<List<string>> LoadFavoritePlayersAsync(string fifaCode)
		{
			var favoritesFilePath = Path.Combine(AssetsFolderPath, FavoritesFileName);

			if (File.Exists(favoritesFilePath))
			{
				var json = await File.ReadAllTextAsync(favoritesFilePath);
				var favorites = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
				if (favorites != null && favorites.TryGetValue(fifaCode, out var playerNames))
				{
					return playerNames;
				}
			}
			return new List<string>();
		}

		private async Task<Dictionary<string, List<string>>> LoadAllFavoritesAsync()
		{
			if (File.Exists(FavoritesFileName))
			{
				var json = await File.ReadAllTextAsync(FavoritesFileName);
				return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json)
					   ?? new Dictionary<string, List<string>>();
			}
			return new Dictionary<string, List<string>>();
		}

		private async Task SaveAllFavoritesAsync(Dictionary<string, List<string>> favorites)
		{
			var json = JsonConvert.SerializeObject(favorites, Formatting.Indented);
			await File.WriteAllTextAsync(FavoritesFileName, json);
		}

		public async Task SaveFavoritePlayersAsync(string championship, string fifaCode, List<string> playerNames)
		{
			var fileName = championship.ToLower() == "men" ? MenFavoritesFileName : WomenFavoritesFileName;
			var favoritesFilePath = Path.Combine(AssetsFolderPath, fileName);
			var favorites = new Dictionary<string, List<string>>();

			if (File.Exists(favoritesFilePath))
			{
				var json = await File.ReadAllTextAsync(favoritesFilePath);
				favorites = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json) ?? new Dictionary<string, List<string>>();
			}

			favorites[fifaCode] = playerNames;

			var updatedJson = JsonConvert.SerializeObject(favorites);
			await File.WriteAllTextAsync(favoritesFilePath, updatedJson);
		}

		public async Task<List<string>> LoadFavoritePlayersAsync(string championship, string fifaCode)
		{
			var fileName = championship.ToLower() == "men" ? MenFavoritesFileName : WomenFavoritesFileName;
			var favoritesFilePath = Path.Combine(AssetsFolderPath, fileName);

			if (File.Exists(favoritesFilePath))
			{
				var json = await File.ReadAllTextAsync(favoritesFilePath);
				var favorites = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
				if (favorites != null && favorites.TryGetValue(fifaCode, out var playerNames))
				{
					return playerNames;
				}
			}
			return new List<string>();
		}


		private static string GetDataLayerPath()
		{
			string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			while (currentPath != null && !Directory.Exists(Path.Combine(currentPath, "DataLayer")))
			{
				currentPath = Directory.GetParent(currentPath)?.FullName;
			}
			return Path.Combine(currentPath, "DataLayer");
		}

		private static readonly string DataLayerPath = GetDataLayerPath();

		public async Task SavePlayerImageAsync(string playerName, byte[] imageData)
		{
			string folderPath = Path.Combine(DataLayerPath, AssetsFolder, ImagesFolder);
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, $"{playerName}.png");
			await File.WriteAllBytesAsync(filePath, imageData);
		}

		public async Task<byte[]> GetPlayerImageAsync(string playerName)
		{
			string filePath = Path.Combine(DataLayerPath, AssetsFolder, ImagesFolder, $"{playerName}.png");
			if (File.Exists(filePath))
			{
				return await File.ReadAllBytesAsync(filePath);
			}
			return null;
		}
	}
}