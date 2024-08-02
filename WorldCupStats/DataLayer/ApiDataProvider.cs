using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DataLayer.Models;
using Newtonsoft.Json;

namespace DataLayer
{
	public class ApiDataProvider : IDataProvider
	{
		private readonly LocalStorageProvider _localStorageProvider;
		private readonly HttpClient _httpClient;
		private const string BaseUrl = "https://worldcup-vua.nullbit.hr";

		public ApiDataProvider()
		{
			_httpClient = new HttpClient();
			_localStorageProvider = new LocalStorageProvider();
		}

		public async Task<List<Team>> GetTeamsAsync(string gender)
		{
			var response = await _httpClient.GetAsync($"{BaseUrl}/{gender}/teams");
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Team>>(json);
		}

		public async Task<Team> GetTeamByFifaCodeAsync(string fifaCode, string gender)
		{
			var teams = await GetTeamsAsync(gender);
			return teams.Find(t => t.FifaCode == fifaCode);
		}

		public async Task<List<Match>> GetMatchesAsync(string gender)
		{
			var response = await _httpClient.GetAsync($"{BaseUrl}/{gender}/matches");
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Match>>(json);
		}

		public async Task<List<Match>> GetMatchesByCountryAsync(string fifaCode, string gender)
		{
			var response = await _httpClient.GetAsync($"{BaseUrl}/{gender}/matches/country?fifa_code={fifaCode}");
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Match>>(json);
		}

		public async Task<List<GroupResult>> GetGroupResultsAsync(string gender)
		{
			var response = await _httpClient.GetAsync($"{BaseUrl}/{gender}/teams/group_results");
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<GroupResult>>(json);
		}

		public async Task<List<Player>> GetPlayersByTeamAsync(string fifaCode, string gender)
		{
			var matches = await GetMatchesByCountryAsync(fifaCode, gender);
			if (matches.Count > 0)
			{
				var firstMatch = matches[0];
				var teamStatistics = firstMatch.HomeTeam.Code == fifaCode ? firstMatch.HomeTeamStatistics : firstMatch.AwayTeamStatistics;
				return teamStatistics.StartingEleven.Concat(teamStatistics.Substitutes).ToList();
			}
			return new List<Player>();
		}

		// Implementacije metoda za postavke i omiljene podatke
		// Ove metode će koristiti lokalno pohranjivanje podataka, pa ćemo ih implementirati u zasebnoj klasi

		public Task SaveSettingsAsync(string key, string value)
		{
			return _localStorageProvider.SaveSettingsAsync(key, value);
		}

		public Task<string> LoadSettingsAsync(string key)
		{
			return _localStorageProvider.LoadSettingsAsync(key);
		}
		public Task SaveFavoriteTeamAsync(string fifaCode)
		{
			return _localStorageProvider.SaveFavoriteTeamAsync(fifaCode);
		}

		public Task<string> LoadFavoriteTeamAsync()
		{
			return _localStorageProvider.LoadFavoriteTeamAsync();
		}

		public Task SaveFavoritePlayersAsync(List<string> playerNames)
		{
			return _localStorageProvider.SaveFavoritePlayersAsync(playerNames);
		}

		public Task<List<string>> LoadFavoritePlayersAsync()
		{
			return _localStorageProvider.LoadFavoritePlayersAsync();
		}

		// Metode za statistiku
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
	}
}