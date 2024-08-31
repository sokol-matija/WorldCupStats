﻿using DataLayer;
using DataLayer.Models;
using System.Drawing.Printing;
using System.Resources;
using System.Text;

namespace WFA_WorldCupStats.Managers
{
    public class UIManager
    {
        private readonly MainForm _form;
        private readonly LogForm _logForm;
        private readonly LocalizationManager _localizationManager;
        private readonly StatisticsManager _statisticsManager;
        private readonly PrintManager _printManager;
        private readonly IDataProvider _dataProvider;

        public UIManager(MainForm form, LogForm logForm, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _form = form;
            _logForm = logForm;
            _localizationManager = new LocalizationManager();
            _statisticsManager = new StatisticsManager();
            _printManager = new PrintManager();
        }

        public void ChangeLanguage(string language)
        {
            _localizationManager.ChangeLanguage(language);
        }

        public void ApplyLocalization()
        {
            _localizationManager.ApplyLocalization(_form);
            _logForm.Log("UI localized successfully.");
        }

        public void ShowLoadingIndicator(bool isLoading)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
                // Disable/enable controls as needed
                _form.cmbTeams.Enabled = !isLoading;
                _form.btnMoveToFavorites.Enabled = !isLoading;
                _form.mnuSettings.Enabled = !isLoading;
                _form.mnuPrint.Enabled = !isLoading;
            });
        }

        public void PopulateTeamsComboBox(List<Team> teams, string favoriteTeam)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.cmbTeams.DataSource = null;
                _form.cmbTeams.DataSource = teams;
                _form.cmbTeams.DisplayMember = "DisplayName";
                _form.cmbTeams.ValueMember = "FifaCode";

                if (!string.IsNullOrEmpty(favoriteTeam))
                {
                    var team = teams.FirstOrDefault(t => t.FifaCode == favoriteTeam);
                    if (team != null)
                    {
                        _form.cmbTeams.SelectedValue = team.FifaCode;
                    }
                }
            });
        }

        public void UpdateMatchesList(List<Match> matches)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.lstMatches.Items.Clear();
                foreach (var match in matches)
                {
                    _form.lstMatches.Items.Add($"{match.HomeTeamCountry} vs {match.AwayTeamCountry} - {match.HomeTeam.Goals}:{match.AwayTeam.Goals}");
                }
            });
        }

        public void UpdateStatisticsList(ListBox listBox, List<PlayerStats> stats)
        {
            _statisticsManager.UpdateStatisticsList(_form, listBox, stats);
        }

        public void UpdatePlayerPanels(List<Player> allPlayers, HashSet<PlayerControl> favoritePlayers)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.pnlAllPlayers.Controls.Clear();
                _form.pnlFavoritePlayers.Controls.Clear();

                foreach (var player in allPlayers)
                {
                    var existingControl = favoritePlayers.FirstOrDefault(fp => fp.Player.Name == player.Name);
                    PlayerControl playerControl;

                    if (existingControl != null)
                    {
                        playerControl = existingControl;
                    }
                    else
                    {
                        playerControl = new PlayerControl(player, _logForm, _dataProvider);
                        playerControl.FavoriteToggled += _form.PlayerControl_FavoriteToggled;
                        playerControl.SelectionToggled += _form.PlayerControl_SelectionToggled;
                    }

                    if (playerControl.IsFavorite)
                    {
                        _form.pnlFavoritePlayers.Controls.Add(playerControl);
                    }
                    else
                    {
                        _form.pnlAllPlayers.Controls.Add(playerControl);
                    }
                }

                ArrangePanelControls(_form.pnlAllPlayers);
                ArrangePanelControls(_form.pnlFavoritePlayers);
            });
        }

        private void ArrangePanelControls(Panel panel)
        {
            int yPosition = 0;
            foreach (Control control in panel.Controls)
            {
                control.Location = new Point(0, yPosition);
                yPosition += control.Height + 5; // 5 pixels spacing
            }
            panel.Refresh();
        }

        public void UpdateMoveToFavoritesButtonVisibility(int selectedPlayersCount)
        {
            _form.Invoke((MethodInvoker)delegate
            {
                _form.btnMoveToFavorites.Visible = selectedPlayersCount > 0;
            });
        }

        public string GenerateStatisticsReport(ListBox topScorers, ListBox yellowCards, ListBox matches)
        {
            return _statisticsManager.GenerateStatisticsReport(topScorers, yellowCards, matches);
        }

        public void ShowPrintPreview(string report)
        {
            _printManager.ShowPrintPreview(report);
        }

        public void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfoMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public DialogResult ShowConfirmationDialog(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}