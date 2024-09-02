using System.Globalization;

namespace WFA_WorldCupStats.Managers
{
    public class LocalizationManager
    {
        public void ChangeLanguage(string language)
        {
            CultureInfo culture;
            if (language == "hr" || language == "Croatian" || language == "Hrvatski")
            {
                culture = new CultureInfo("hr-HR");
            }
            else
            {
                culture = new CultureInfo("en-US");
            }

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public void ApplyLocalization(MainForm form)
        {
            form.Text = Strings.MainFormTitle;

            LocalizeMenuItems(form);
            LocalizeLabels(form);
            LocalizeButtons(form);

            LocalizePlayerControls(form.pnlAllPlayers);
            LocalizePlayerControls(form.pnlFavoritePlayers);
        }

        private void LocalizeMenuItems(MainForm form)
        {
            form.mnuSettings.Text = Strings.SettingsMenuItem;
            form.mnuPrint.Text = Strings.PrintMenuItem;
            form.mnuPrintStatistics.Text = Strings.PrintStatisticsMenuItem;
        }

        private void LocalizeLabels(MainForm form)
        {
            form.lblTopScorers.Text = Strings.TopScorersLabel;
            form.lblYellowCards.Text = Strings.YellowCardsLabel;
            form.lblAllPlayers.Text = Strings.AllPlayersLabel;
            form.lblFavoritePlayers.Text = Strings.FavoritePlayersLabel;
            form.lblMatches.Text = Strings.MatchesLabel;
            form.lblChooseTeam.Text = Strings.ChooseTeamLabel;
        }

        private void LocalizeButtons(MainForm form)
        {
            form.btnMoveToFavorites.Text = Strings.MoveToFavorites;
        }

        private void LocalizePlayerControls(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is PlayerControl playerControl)
                {
                    playerControl.ApplyLocalization();
                }
            }
        }
    }
}