using DataLayer;

namespace WFA_WorldCupStats
{
	public class SettingsManager
	{
		private readonly IDataProvider _dataProvider;
		private readonly LogForm _logForm;

		public string SelectedChampionship { get; private set; }
		public string SelectedLanguage { get; private set; }
		public string FavoriteTeam { get; private set; }

		public SettingsManager(IDataProvider dataProvider, LogForm logForm)
		{
			_dataProvider = dataProvider;
			_logForm = logForm;
		}

		public async Task<bool> AreSettingsDefinedAsync()
		{
			try
			{
				var championship = await _dataProvider.LoadSettingsAsync("Championship");
				var language = await _dataProvider.LoadSettingsAsync("Language");

				return !string.IsNullOrEmpty(championship) && !string.IsNullOrEmpty(language);
			}
			catch (Exception ex)
			{
				_logForm.Log($"Error checking settings: {ex.Message}");
				return false;
			}
		}

		public async Task LoadInitialSettingsAsync()
		{
			try
			{
				SelectedChampionship = await _dataProvider.LoadSettingsAsync("Championship") ?? "men";
				SelectedLanguage = await _dataProvider.LoadSettingsAsync("Language") ?? "en";
				FavoriteTeam = await _dataProvider.LoadFavoriteTeamAsync();
			}
			catch (Exception ex)
			{
				_logForm.Log($"Error loading settings: {ex.Message}");
				SelectedChampionship = "men";
				SelectedLanguage = "en";
				FavoriteTeam = null;
			}
		}

		public async Task<bool> ShowInitialSettingsFormAsync()
		{
			if (await AreSettingsDefinedAsync())
			{
				await LoadInitialSettingsAsync();
				return true;
			}

			using (var settingsForm = new InitialSettingsForm(_dataProvider, _logForm))
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
			using (var settingsForm = new InitialSettingsForm(_dataProvider, _logForm))
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
			if (string.IsNullOrEmpty(fifaCode))
			{
				throw new ArgumentException("FIFA code cannot be null or empty", nameof(fifaCode));
			}

			try
			{
				await _dataProvider.SaveFavoriteTeamAsync(fifaCode);
				FavoriteTeam = fifaCode;
			}
			catch (Exception ex)
			{
				_logForm.Log($"Error saving favorite team: {ex.Message}");
				throw;
			}
		}
	}
}