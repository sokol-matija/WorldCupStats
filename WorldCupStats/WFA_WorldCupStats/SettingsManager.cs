using DataLayer;

namespace WFA_WorldCupStats
{
	public class SettingsManager
	{
		private readonly IDataProvider _dataProvider;

		public string SelectedChampionship { get; private set; }
		public string SelectedLanguage { get; private set; }
		public string FavoriteTeam { get; private set; }

		public SettingsManager(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}

		public async Task LoadInitialSettingsAsync()
		{
			SelectedChampionship = await _dataProvider.LoadSettingsAsync("Championship");
			SelectedLanguage = await _dataProvider.LoadSettingsAsync("Language");
			FavoriteTeam = await _dataProvider.LoadFavoriteTeamAsync();
		}

		public async Task<bool> ShowInitialSettingsFormAsync()
		{
			using (var settingsForm = new InitialSettingsForm(_dataProvider))
			{
				if (settingsForm.ShowDialog() == DialogResult.OK)
				{
					await LoadInitialSettingsAsync();
					return true;
				}
			}
			return false;
		}

		public async Task<bool> ShowSettingsFormAsync()
		{
			using (var settingsForm = new InitialSettingsForm(_dataProvider))
			{
				settingsForm.SetCurrentSettings(SelectedChampionship, SelectedLanguage);
				if (settingsForm.ShowDialog() == DialogResult.OK)
				{
					await LoadInitialSettingsAsync();
					return true;
				}
			}
			return false;
		}

		public async Task SaveFavoriteTeamAsync(string fifaCode)
		{
			await _dataProvider.SaveFavoriteTeamAsync(fifaCode);
			FavoriteTeam = fifaCode;
		}
	}
}