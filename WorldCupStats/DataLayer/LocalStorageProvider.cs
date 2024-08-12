using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer
{
	public class LocalStorageProvider
	{
		private const string SettingsFile = "settings.json";
		private const string FavoritesFile = "favorites.json";
		private const string FavoriteTeamFile = "favorite_team.json";

		public async Task SaveSettingsAsync(string key, string value)
		{
			var settings = await LoadAllSettingsAsync();
			settings[key] = value;
			await File.WriteAllTextAsync(SettingsFile, JsonConvert.SerializeObject(settings));
		}

		public async Task<string> LoadSettingsAsync(string key)
		{
			var settings = await LoadAllSettingsAsync();
			return settings.TryGetValue(key, out var value) ? value : null;
		}

		private async Task<Dictionary<string, string>> LoadAllSettingsAsync()
		{
			if (File.Exists(SettingsFile))
			{
				var json = await File.ReadAllTextAsync(SettingsFile);
				return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}
			return new Dictionary<string, string>();
		}

		public async Task SaveFavoriteTeamAsync(string fifaCode)
		{
			await File.WriteAllTextAsync(FavoriteTeamFile, fifaCode);
		}

		public async Task<string> LoadFavoriteTeamAsync()
		{
			if (File.Exists(FavoriteTeamFile))
			{
				return await File.ReadAllTextAsync(FavoriteTeamFile);
			}
			return null;
		}

		public async Task SaveFavoritePlayersAsync(List<string> playerNames)
		{
			var favorites = await LoadAllFavoritesAsync();
			favorites["FavoritePlayers"] = JsonConvert.SerializeObject(playerNames);
			await File.WriteAllTextAsync(FavoritesFile, JsonConvert.SerializeObject(favorites));
		}

		public async Task<List<string>> LoadFavoritePlayersAsync()
		{
			var favorites = await LoadAllFavoritesAsync();
			if (favorites.TryGetValue("FavoritePlayers", out var value))
			{
				return JsonConvert.DeserializeObject<List<string>>(value);
			}
			return new List<string>();
		}

		private async Task<Dictionary<string, string>> LoadAllFavoritesAsync()
		{
			if (File.Exists(FavoritesFile))
			{
				var json = await File.ReadAllTextAsync(FavoritesFile);
				return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}
			return new Dictionary<string, string>();
		}
	}
}