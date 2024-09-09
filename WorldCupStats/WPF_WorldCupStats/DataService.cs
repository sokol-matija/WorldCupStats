using DataLayer;
using DataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPF_WorldCupStats
{
	public class DataService
	{
		private readonly IDataProvider _dataProvider;

		public DataService()
		{
			_dataProvider = DataProviderFactory.CreateDataProvider();
		}

		public async Task<List<Team>> GetTeamsAsync(string championship)
		{
			return await _dataProvider.GetTeamsAsync(championship);

		}

		public async Task<List<Match>> GetMatchesAsync(string championship)
		{
			return await _dataProvider.GetMatchesAsync(championship);
		}

		public async Task<List<Player>> GetPlayersByTeamAsync(string fifaCode, string championship)
		{
			return await _dataProvider.GetPlayersByTeamAsync(fifaCode, championship);
		}

		public async Task<string> LoadSettingsAsync(string key)
		{
			return await _dataProvider.LoadSettingsAsync(key);
		}

		public async Task SaveSettingsAsync(string key, string value)
		{
			await _dataProvider.SaveSettingsAsync(key, value);
		}

		public async Task GetPlyaerImageAsync(string playerName)
		{
			await _dataProvider.GetPlayerImageAsync(playerName);
		}
	}
}