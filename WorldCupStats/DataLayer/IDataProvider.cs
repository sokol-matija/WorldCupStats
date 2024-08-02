using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public interface IDataProvider
	{
		Task<List<Team>> GetTeamsAsync(string gender);
		Task<Team> GetTeamByFifaCodeAsync(string fifaCode, string gender);

		Task<List<Match>> GetMatchesAsync(string gender);
		Task<List<Match>> GetMatchesByCountryAsync(string fifaCode, string gender);

		Task<List<GroupResult>> GetGroupResultsAsync(string gender);

		Task<List<Player>> GetPlayersByTeamAsync(string fifaCode, string gender);

		Task SaveSettingsAsync(string key, string value);
		Task<string> LoadSettingsAsync(string key);

		Task SaveFavoriteTeamAsync(string fifaCode);
		Task<string> LoadFavoriteTeamAsync();
		Task SaveFavoritePlayersAsync(List<string> playerNames);
		Task<List<string>> LoadFavoritePlayersAsync();

		Task<List<PlayerStats>> GetTopScorersAsync(string gender, int count);
		Task<List<PlayerStats>> GetYellowCardsAsync(string gender, int count);
		Task<List<Match>> GetMatchesByAttendanceAsync(string gender, int count);
	}
}
