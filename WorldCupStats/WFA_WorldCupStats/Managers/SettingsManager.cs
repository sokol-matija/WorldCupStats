using DataLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WFA_WorldCupStats.Managers
{
    public class SettingsManager
    {
        private readonly IDataProvider _dataProvider;
        private readonly LogForm _logForm;

        public string? SelectedChampionship { get; private set; }
        public string? SelectedLanguage { get; private set; }
        public string? FavoriteTeam { get; private set; }

        public event EventHandler? SettingsChanged;

        public SettingsManager(IDataProvider dataProvider, LogForm logForm)
        {
            _dataProvider = dataProvider;
            _logForm = logForm;
            SelectedChampionship = string.Empty;
            SelectedLanguage = string.Empty;
            FavoriteTeam = string.Empty;
        }

        protected virtual void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadInitialSettingsAsync()
        {
            try
            {
                SelectedChampionship = await _dataProvider.LoadSettingsAsync("Championship") ?? SettingsConstants.MenChampionship;
                SelectedLanguage = await _dataProvider.LoadSettingsAsync("Language") ?? SettingsConstants.EnglishLanguage;
                FavoriteTeam = await _dataProvider.LoadFavoriteTeamAsync();
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error loading initial settings: {ex.Message}");
                SelectedChampionship = SettingsConstants.MenChampionship;
                SelectedLanguage = SettingsConstants.EnglishLanguage;
                FavoriteTeam = null;
            }
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

        public async Task<bool> ShowInitialSettingsFormAsync()
        {
            if (await AreSettingsDefinedAsync())
            {
                await LoadInitialSettingsAsync();
                return true;
            }

            using (var settingsForm = new InitialSettingsForm(this))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadInitialSettingsAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task SaveSettingsAsync(string championship, string language)
        {
            if (!SettingsValidator.IsValidChampionship(championship) || !SettingsValidator.IsValidLanguage(language))
            {
                throw new ArgumentException("Invalid championship or language value");
            }

            try
            {
                await _dataProvider.SaveSettingsAsync("Championship", championship);
                await _dataProvider.SaveSettingsAsync("Language", language);
                SelectedChampionship = championship;
                SelectedLanguage = language;
                _logForm.Log($"Settings saved: Championship={championship}, Language={language}");
                OnSettingsChanged();
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error saving settings: {ex.Message}");
                throw;
            }
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
                _logForm.Log($"Favorite team saved: {fifaCode}");
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error saving favorite team: {ex.Message}");
                throw;
            }
        }

        public async Task SaveFavoritePlayersAsync(List<string> playerNames)
        {
            try
            {
                await _dataProvider.SaveFavoritePlayersAsync(SelectedChampionship, FavoriteTeam, playerNames);
                _logForm.Log($"Favorite players saved for {FavoriteTeam} in {SelectedChampionship} championship");
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error saving favorite players: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> LoadFavoritePlayersAsync()
        {
            try
            {
                var players = await _dataProvider.LoadFavoritePlayersAsync(SelectedChampionship, FavoriteTeam);
                _logForm.Log($"Loaded {players.Count} favorite players for {FavoriteTeam} in {SelectedChampionship} championship");
                return players;
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error loading favorite players: {ex.Message}");
                return new List<string>();
            }
        }

        public string GetChampionshipDisplayName()
        {
            return SelectedChampionship == SettingsConstants.MenChampionship ? Strings.MensChampionship : Strings.WomensChampionship;
        }

        public string GetLanguageDisplayName()
        {
            return SelectedLanguage == SettingsConstants.EnglishLanguage ? Strings.EnglishLanguage : Strings.CroatianLanguage;
        }

        public async Task<bool> ShowSettingsFormAsync()
        {
            using (var settingsForm = new InitialSettingsForm(this))
            {
                settingsForm.SetCurrentSettings();
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadInitialSettingsAsync();
                    return true;
                }
            }
            return false;
        }
    }

    public static class SettingsConstants
    {
        public const string MenChampionship = "men";
        public const string WomenChampionship = "women";
        public const string EnglishLanguage = "en";
        public const string CroatianLanguage = "hr";
    }

    public static class SettingsValidator
    {
        public static bool IsValidChampionship(string championship)
        {
            return championship == SettingsConstants.MenChampionship || championship == SettingsConstants.WomenChampionship;
        }

        public static bool IsValidLanguage(string language)
        {
            return language == SettingsConstants.EnglishLanguage || language == SettingsConstants.CroatianLanguage;
        }
    }
}