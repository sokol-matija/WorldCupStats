using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WFA_WorldCupStats.Managers
{
    public class FavoritePlayerManager
    {
        private readonly IDataProvider _dataProvider;
        private readonly SettingsManager _settingsManager;
        private readonly LogForm _logForm;

        public FavoritePlayerManager(IDataProvider dataProvider, SettingsManager settingsManager, LogForm logForm)
        {
            _dataProvider = dataProvider;
            _settingsManager = settingsManager;
            _logForm = logForm;
        }

        public async Task<List<string>> LoadFavoritePlayersAsync()
        {
            try
            {
                var favoritePlayerNames = await _settingsManager.LoadFavoritePlayersAsync();
                _logForm.Log($"Loaded {favoritePlayerNames.Count} favorite players");
                return favoritePlayerNames;
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error loading favorite players: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task SaveFavoritePlayersAsync(List<string> favoritePlayerNames)
        {
            try
            {
                await _settingsManager.SaveFavoritePlayersAsync(favoritePlayerNames);
                _logForm.Log($"Saved {favoritePlayerNames.Count} favorite players");
            }
            catch (Exception ex)
            {
                _logForm.Log($"Error saving favorite players: {ex.Message}");
            }
        }
    }
}